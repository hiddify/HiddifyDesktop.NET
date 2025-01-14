﻿using System;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Clasharp.ViewModels;
using ReactiveUI;

namespace Clasharp.Views;

public partial class ProfileEditWindow : WindowBase<ProfileEditViewModel>
{
    public ProfileEditWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d =>
        {
            d(ViewModel.Save.Subscribe(Close));
            d(ViewModel.ShowOpenFileDialog.RegisterHandler(ShowFileDialog));
            Title = ViewModel.IsCreate ? Clasharp.Resources.txtNewProfile : Clasharp.Resources.txtEditProfile;
        });
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            AcrylicBorder.IsVisible = false;
        }
    }

    private async Task ShowFileDialog(InteractionContext<Unit, string?> interaction)
    {
        var openFileDialog = new OpenFileDialog
        {
            AllowMultiple = false
        };
        var files = await openFileDialog.ShowAsync(this);
        interaction.SetOutput(files?.FirstOrDefault());
    }
}