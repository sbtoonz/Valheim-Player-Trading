﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerTrading.GUI
{
    public abstract class TradeWindow : MonoBehaviour
    {
        public enum WindowPositionType
        {
            LEFT,
            RIGHT
        }

        protected bool Initialised { get; set; }
        protected bool WindowActive { get; set; }
        protected string Name { get; set; }
        protected WindowPositionType WindowPosition { get; set; }
        protected RectTransform TradeWindowGUIRT { get; set; }
        protected Player LocalPlayer { get; set; }
        protected Inventory WindowInventory { get; set; }
        protected Image WindowBackground { get; set; }
        protected Color OriginalBkgColor { get; set; }

        private Vector2 _defaultRTAnchorMin;
        private Vector2 _defaultRTAnchorMax;
        private Vector2 _defaultRTAnchorPos;

        private void Awake() {}

        private void SubscribeToEvents()
        {
            PlayerTradingMain.OnLocalPlayerChanged += OnNewLocalPlayer;
        }
        
        private void UnsubscribeToEvents()
        {
            PlayerTradingMain.OnLocalPlayerChanged -= OnNewLocalPlayer;
        }

        private void OnDestroy()
        {
            UnsubscribeToEvents();
        }

        public void OnNewLocalPlayer()
        {
            LocalPlayer = Player.m_localPlayer;
        }

        public bool IsInitialised() => Initialised;

        public bool IsActive() => WindowActive;

        public Inventory GetInventory() => WindowInventory;

        public abstract void Initialise(string tradeWindowName, WindowPositionType windowPosition);

        public abstract void ResetForNewInstance();

        public abstract void ResetDefaultPosition();

        public abstract void Refresh();

        public abstract List<ItemDrop.ItemData> GetItems();

        public abstract void Show();

        public abstract void Hide();

        public abstract void OnTradeCancelled();

        public abstract bool IsShowing();

        public abstract void DestroyTradeWindow();

        public abstract void SetAsAccepted(bool accepted);

        protected void SetDefaultPosition()
        {
            _defaultRTAnchorMin = TradeWindowGUIRT.anchorMin;
            _defaultRTAnchorMax = TradeWindowGUIRT.anchorMax;
            _defaultRTAnchorPos = TradeWindowGUIRT.anchoredPosition;
        }

        protected void UpdatePosition()
        {
            float width, height;
            Vector2 newPos;

            float guiScale = PlayerPrefs.GetFloat("GuiScale", 1f);

            float xOffset = (Screen.width / 30f) * guiScale;
            float yOffset = (Screen.height / 30f) * guiScale;

            switch (WindowPosition)
            {
                case WindowPositionType.LEFT:
                    width = (Screen.width / 2) - xOffset;
                    height = (Screen.height / 2) - yOffset;
                    newPos = Camera.main.ScreenToViewportPoint(new Vector3(width, height, 0f));
                    TradeWindowGUIRT.anchorMin = newPos;
                    TradeWindowGUIRT.anchorMax = newPos;
                    TradeWindowGUIRT.anchoredPosition = newPos;
                    break;
                case WindowPositionType.RIGHT:
                    width = (Screen.width / 2) + xOffset;
                    height = (Screen.height / 2) - yOffset;
                    newPos = Camera.main.ScreenToViewportPoint(new Vector3(width, height, 0f));
                    TradeWindowGUIRT.anchorMin = newPos;
                    TradeWindowGUIRT.anchorMax = newPos;
                    TradeWindowGUIRT.anchoredPosition = newPos;
                    break;
                default:
                    break;
            }
        }

        protected void ResetPosition()
        {
            TradeWindowGUIRT.anchorMin = _defaultRTAnchorMin;
            TradeWindowGUIRT.anchorMax = _defaultRTAnchorMax;
            TradeWindowGUIRT.anchoredPosition = _defaultRTAnchorPos;
        }

    }
}
