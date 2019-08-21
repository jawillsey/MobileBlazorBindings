﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BlaxamarinSample.Services;
using BlaxamarinSample.Views;

namespace BlaxamarinSample
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            Blaxamarin.Framework.Blaxamarin.Run<Blaxample>(this);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
