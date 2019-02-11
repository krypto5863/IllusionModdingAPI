﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MakerAPI
{
    /// <summary>
    /// Use with AddLoadToggle
    /// </summary>
    public class MakerCoordinateLoadToggle : BaseEditableGuiEntry<bool>
    {
        private const int TotalWidth = 380 + 292;

        private static readonly List<MakerCoordinateLoadToggle> Toggles = new List<MakerCoordinateLoadToggle>();
        private static Transform _baseToggle;
        private static GameObject _root;

        private static int _createdCount;
        private static List<RectTransform> _baseToggles;

        public MakerCoordinateLoadToggle(string text, bool initialValue = true) : base(null, initialValue, null)
        {
            Text = text;
        }

        public string Text { get; }
        public static bool AnyEnabled => Toggles.Any(x => x.Value);
        internal static Button LoadButton { get; private set; }

        internal static void CreateCustomToggles()
        {
            foreach (var toggle in Toggles)
                toggle.CreateControl(_root.transform);

            var singleWidth = TotalWidth / (_baseToggles.Count + Toggles.Count);
            for (var index = 0; index < _baseToggles.Count; index++)
            {
                var baseToggle = _baseToggles[index];
                baseToggle.localPosition = new Vector3(-380 + singleWidth * index, 52, 0);
                baseToggle.offsetMax = new Vector2(baseToggle.offsetMin.x + singleWidth, baseToggle.offsetMin.y + 26);
            }
        }

        protected override GameObject OnCreateControl(Transform loadBoxTransform)
        {
            var copy = Object.Instantiate(_baseToggle, _root.transform);
            copy.name = "tglItem" + GuiApiNameAppendix;

            var tgl = copy.GetComponentInChildren<Toggle>();
            BufferedValueChanged.Subscribe(b => tgl.isOn = b);
            tgl.onValueChanged.AddListener(SetNewValue);
            tgl.image.raycastTarget = true;
            tgl.graphic.raycastTarget = true;

            var txt = copy.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = Text;

            var singleWidth = TotalWidth / (_baseToggles.Count + Toggles.Count);

            var rt = copy.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(-380 + singleWidth * _createdCount, 52, 0);
            rt.offsetMax = new Vector2(rt.offsetMin.x + singleWidth, rt.offsetMin.y + 26);

            copy.gameObject.SetActive(true);
            _createdCount++;

            return copy.gameObject;
        }

        protected internal override void Initialize() { }

        internal static MakerCoordinateLoadToggle AddLoadToggle(MakerCoordinateLoadToggle toggle)
        {
            Toggles.Add(toggle);
            return toggle;
        }

        internal static void Reset()
        {
            foreach (var toggle in Toggles)
                toggle.Dispose();
            Toggles.Clear();
            _createdCount = 0;
            LoadButton = null;
        }

        internal static void Setup()
        {
            Reset();

            _root = GetRootObject();

            _baseToggles = _root.transform.Cast<Transform>()
                .Where(x => x.name.StartsWith("tglItem0"))
                .Select(x => x.GetComponent<RectTransform>())
                .ToList();

            _createdCount = _baseToggles.Count;
            
            _baseToggle = _root.transform.Find("tglItem01");

            /*var allon = _root.transform.Find("btnAllOn");
            allon.GetComponentInChildren<Button>().onClick.AddListener(OnAllOn);
            var alloff = _root.transform.Find("btnAllOff");
            alloff.GetComponentInChildren<Button>().onClick.AddListener(OnAllOff);*/

            LoadButton = _root.transform.parent.Find("btnLoad").GetComponent<Button>();
        }

        private static GameObject GetRootObject()
        {
            return GameObject.Find("CustomScene/CustomRoot/FrontUIGroup/CustomUIGroup/CvsMenuTree/06_SystemTop/cosFileControl/charaFileWindow/WinRect/CoordinateLoad/Select");
        }

        /*private static void OnAllOff()
        {
            foreach (var toggle in Toggles)
            {
                if (toggle != null)
                    toggle.Value = false;
            }
        }

        private static void OnAllOn()
        {
            foreach (var toggle in Toggles)
            {
                if (toggle != null)
                    toggle.Value = true;
            }
        }*/
    }
}