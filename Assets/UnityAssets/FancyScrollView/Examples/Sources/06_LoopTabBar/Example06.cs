/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example06
{
    class Example06 : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;
        [SerializeField] Text selectedItemInfo = default;
        [SerializeField] List<Window> windows = new List<Window>();
        [SerializeField] ChapterScriptAble chapterScriptAble;
        [SerializeField] private Transform ChapterParent;
        [SerializeField] GraphicRaycaster graphicRaycasterAdd;
        Window currentWindow;
        [SerializeField] private Window windowPrefab;

        private LoadSceneManager loadSceneManager;


        private void Awake() {
            loadSceneManager = LoadSceneManager.Instance;
        }

        void Start()
        {
            foreach(ChapterScriptAble.Chapter chapter in chapterScriptAble.chapters) {
                Window window = Instantiate(windowPrefab);
                windows.Add(window);
                window.GetComponent<SlideScreenTransition>().graphicRaycaster = graphicRaycasterAdd; 
                window.transform.SetParent(ChapterParent, true); 
                window.index = chapter.index;

            }

            scrollView.OnSelectionChanged(OnSelectionChanged);

            var items = Enumerable.Range(0, windows.Count)
                .Select(i => new ItemData($"Tab {i}"))
                .ToList();

            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }

        void OnSelectionChanged(int index, MovementDirection direction)
        {
            selectedItemInfo.text = $"Selected chapter: index {index}";

            if (currentWindow != null)
            {
                currentWindow.Out(direction);
                currentWindow = null;
            }

            if (index >= 0 && index < windows.Count)
            {
                currentWindow = windows[index];
                currentWindow.In(direction);
            }
        }

        public void PlayChapter() {
            loadSceneManager.LoadScene(currentWindow.index);
        }
    }
}
