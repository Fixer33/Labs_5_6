﻿using LAB_5_6.ViewModels;
using LAB_5_6.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LAB_5_6
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
