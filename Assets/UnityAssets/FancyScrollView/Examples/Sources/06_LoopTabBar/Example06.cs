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
        [SerializeField] List<Chapter> chapters = new List<Chapter>();
        [SerializeField] ChapterScriptAble chapterScriptAble;
        [SerializeField] private Transform ChapterParent;
        [SerializeField] GraphicRaycaster graphicRaycasterAdd;
        Chapter currentChapter;
        [SerializeField] private Chapter chapterPrefab;

        private LoadSceneManager loadSceneManager;


        private void Awake() {
            loadSceneManager = LoadSceneManager.Instance;
        }

        void Start()
        {
            for(int i = 0;  i< chapterScriptAble.chapters.Length; i++){
                Chapter chapter = Instantiate(chapterPrefab);
                chapter.gameObject.SetActive(i==0);
                chapters.Add(chapter);
                chapter.GetComponent<SlideScreenTransition>().graphicRaycaster = graphicRaycasterAdd; 
                chapter.transform.SetParent(ChapterParent, false); 
                chapter.index = i;
                chapter.nameChapter = chapterScriptAble.chapters[i].nameChapter;
                chapter.thumb = chapterScriptAble.chapters[i].thumb;
            }


            scrollView.OnSelectionChanged(OnSelectionChanged);

            var items = Enumerable.Range(0, chapters.Count)
                .Select(i => new ItemData($"Tab {i}"))
                .ToList();

            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }

        void OnSelectionChanged(int index, MovementDirection direction)
        {

            if (currentChapter != null)
            {
                currentChapter.Out(direction);
                currentChapter = null;
            }

            if (index >= 0 && index < chapters.Count)
            {
                currentChapter = chapters[index];
                currentChapter.In(direction);
            }


            selectedItemInfo.text = $"Selected chapter: {currentChapter.nameChapter}";
        }

        public void PlayChapter() {
            loadSceneManager.LoadScene(currentChapter.index);
            Debug.Log(currentChapter.index);
        }
    }
}
