﻿using System;
using System.Collections.Generic;
using Set.ViewModels;
using Set.Models;
using Xamarin.Forms;
using Set.Resx;

namespace Set
{
	public partial class AboutPage : ContentPage
	{
        protected AboutViewModel _viewModel;
        public AboutViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = new AboutViewModel(Navigation);
                }
                return _viewModel;
            }
            set
            {
                _viewModel = value;
            }
        }

        public AboutPage()
		{
			this.InitializeComponent ();
			this.BindingContext = ViewModel;
		}

		protected async override void OnAppearing()
		{
            base.OnAppearing();
            BindingContext = ViewModel;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}
	}
}
