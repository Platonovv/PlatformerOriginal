﻿using System.Collections.Generic;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.UI.HUD.Dialogs;
using PixelCrew.UI.Widgets;
using PixelCrew.UI.Windows;
using UnityEngine;

namespace PixelCrew.UI.Localization
{
    public class LocalizationWindow : AnimatedWindow
    {
        [SerializeField] private Transform _container;
        [SerializeField] private LocaleItemWidget _prefab;
        
        private DataGroup<LocaleInfo, LocaleItemWidget> _dataGroup;


        private readonly string[] _supportedLocales = new[] {"en", "ru", "ec"};
        
        protected override void Start()
        {
            base.Start();
            _dataGroup = new DataGroup<LocaleInfo, LocaleItemWidget>(_prefab, _container);
            _dataGroup.SetData(ComposeData());
            
        }

        private List<LocaleInfo> ComposeData()
        {
            var data = new List<LocaleInfo>();
            foreach (var locale in _supportedLocales)
            {
                data.Add(new LocaleInfo{LocaleId = locale});
            }
            return data;
        }

        public void OnSelected(string selectedLocale)
        {
            LocalizationManager.I.SetLocale(selectedLocale);
        }
    }
}