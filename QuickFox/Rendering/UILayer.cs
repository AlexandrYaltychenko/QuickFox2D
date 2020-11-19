using System;
using System.Collections.Generic;
using System.Linq;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.UI;

namespace QuickFox.Rendering
{
    public class UILayer : UIEntity
    {
        private readonly IUIManager _uiManager;
        private readonly IComponentManager _componentManager;
        public int ChildCount { get; private set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public LayoutOrientation Orientation { get; set; }

        public HorizontalGravity HorizontalGravity { get; set; }

        public VerticalGravity VerticalGravity { get; set; }

        internal UILayer(UIManager uiManager, IEntityManager entityManager, IComponentManager componentManager) : base(entityManager)
        {
            IsActive = true;
            _uiManager = uiManager;
            _componentManager = componentManager;
        }

        internal override void Measure(int availableWidth, int availableHeight)
        {
            var remainingWidth = GetRemainingSizeFromAvailable(WidthRequest, availableWidth);
            var remainingHeight = GetRemainingSizeFromAvailable(HeightRequest, availableHeight);

            var contentWidth = 0;
            var contentHeight = 0;

            foreach (var element in _uiManager.GetUIElementsForLayer(this.Id))
            {
                element.Measure(remainingWidth, remainingHeight);
                var elementHorizontalMargins = element.MarginLeft + element.MarginRight;
                var elementVerticalMargins = element.MarginTop + element.MarginBottom;

                if (Orientation == LayoutOrientation.Vertical)
                {
                    remainingHeight -= element.Bounds.Height + elementHorizontalMargins;
                    contentHeight += element.Bounds.Height + elementHorizontalMargins;
                    contentWidth = Math.Max(contentWidth, element.Bounds.Width + elementHorizontalMargins);
                }
                else
                {
                    remainingWidth -= element.Bounds.Width + elementVerticalMargins;
                    contentWidth += element.Bounds.Width + elementVerticalMargins;
                    contentHeight = Math.Max(contentHeight, element.Bounds.Height + elementVerticalMargins);
                }
            }

            Bounds.Width = GetResultingSizeFromRequestAndAvailable(contentWidth, WidthRequest, availableWidth);
            Bounds.Height = GetResultingSizeFromRequestAndAvailable(contentHeight, HeightRequest, availableHeight);
        }

        public void RequestLayout(int parentWidth, int parentHeight)
        {
            var availableWidth = GetRemainingSizeFromAvailable(WidthRequest, parentWidth);
            var availableHeight = GetRemainingSizeFromAvailable(HeightRequest, parentHeight);

            Measure(availableWidth, availableHeight);

            var newX = 0;
            var newY = 0;

            if (VerticalAlignment == VerticalAlignment.Bottom)
            {
                newY += parentHeight - Bounds.Height - MarginBottom;
            }
            else if (VerticalAlignment == VerticalAlignment.Center)
            {
                newY += (parentHeight - Bounds.Height) / 2;
            }
            else
            {
                newY += MarginTop;
            }

            if (HorizontalAlignment == HorizontalAlignment.Right)
            {
                newX += parentWidth - Bounds.Width - MarginRight;
            }
            else if (HorizontalAlignment == HorizontalAlignment.Center)
            {
                newX += (parentWidth - Bounds.Width) / 2;
            }
            else
            {
                newX += MarginLeft;
            }

            Layout(newX, newY);

        }

        internal override void Layout(int newX, int newY)
        {
            Bounds.IsValid = true;
            Bounds.X = newX;
            Bounds.Y = newY;

            var childs = _uiManager.GetUIElementsForLayer(this.Id);

            LayoutChilds(childs);
        }

        internal override void Invalidate()
        {
            base.Invalidate();

            Bounds.IsValid = false;

            foreach (var element in _uiManager.GetUIElementsForLayer(this.Id))
            {
                if (element.Bounds.IsValid)
                {
                    element.Invalidate();
                }
            }

            if (ParentView?.Bounds.IsValid ?? false)
            {
                ParentView?.Invalidate();
            }
        }

        private void LayoutChilds(IEnumerable<UIEntity> childs)
        {
            var childrenWidth = childs.Any() ? childs.Sum(c => c.Bounds.Width + c.MarginLeft + c.MarginRight) : 0;
            var childrenHeight = childs.Any() ? childs.Sum(c => c.Bounds.Height + c.MarginTop + c.MarginBottom) : 0;
            var maxWidth = childs.Any() ? childs.Max(c => c.Bounds.Width + c.MarginLeft + c.MarginRight) : 0;
            var maxHeight = childs.Any() ? childs.Max(c => c.Bounds.Height + c.MarginTop + c.MarginBottom) : 0;

            if (Orientation == LayoutOrientation.Horizontal)
            {
                var startingX = Bounds.X;

                if (HorizontalGravity == HorizontalGravity.Left)
                {
                    startingX = Bounds.X;
                }
                else if (HorizontalGravity == HorizontalGravity.Right)
                {
                    startingX = Bounds.X + Bounds.Width - childrenWidth;
                }
                else if (HorizontalGravity == HorizontalGravity.Center)
                {
                    startingX = Bounds.X + (Bounds.Width - childrenWidth) / 2;
                }

                foreach (var child in childs.OrderBy(c => c.OrderInParent))
                {
                    var childY = 0;

                    switch (child.VerticalAlignment)
                    {
                        default:
                            childY = Bounds.Y + child.MarginTop;
                            break;
                        case VerticalAlignment.Center:
                            childY = Bounds.Y + (Bounds.Height - child.Bounds.Height) / 2 + child.MarginTop - child.MarginBottom;
                            break;
                        case VerticalAlignment.Bottom:
                            childY = Bounds.Y + Bounds.Height - child.Bounds.Height - child.MarginBottom;
                            break;
                    }

                    child.Layout(startingX + child.MarginLeft, childY);
                    startingX += child.Bounds.Width;
                }

            }
            else if (Orientation == LayoutOrientation.Vertical)
            {
                var startingY = Bounds.Y;

                if (VerticalGravity == VerticalGravity.Top)
                {
                    startingY = Bounds.Y;
                }
                else if (VerticalGravity == VerticalGravity.Bottom)
                {
                    startingY = Bounds.Y + Bounds.Height - childrenHeight;
                }
                else if (VerticalGravity == VerticalGravity.Center)
                {
                    startingY = Bounds.Y + (Bounds.Height - childrenHeight) / 2;
                }

                foreach (var child in childs)
                {
                    var childX = 0;

                    switch (child.HorizontalAlignment)
                    {
                        default:
                            childX = Bounds.X + child.MarginLeft;
                            break;
                        case HorizontalAlignment.Center:
                            childX = Bounds.X + (Bounds.Width - child.Bounds.Width) / 2 + child.MarginLeft - child.MarginRight;
                            break;
                        case HorizontalAlignment.Right:
                            childX = Bounds.X + Bounds.Width - child.Bounds.Width - child.MarginBottom;
                            break;
                    }

                    child.Layout(childX, startingY + child.MarginTop);
                    startingY += child.Bounds.Height;
                }
            }
        }

        private int GetResultingSizeFromRequestAndAvailable(int childrenSize, int sizeRequest, int availableSize)
        {
            if (sizeRequest == LayoutConstants.MatchParent)
                return availableSize;

            if (sizeRequest == LayoutConstants.WrapContent)
                return childrenSize;

            return Math.Min(sizeRequest, availableSize);
        }

        private int GetRemainingSizeFromAvailable(int sizeRequest, int availableSize)
        {
            if (sizeRequest == LayoutConstants.WrapContent || sizeRequest == LayoutConstants.MatchParent)
                return availableSize;

            return sizeRequest;
        }

        public UIElementBuilder<T> GetBuilder<T>() where T : UIEntity
        {
            return new UIElementBuilder<T>(_uiManager, _componentManager, Id, SceneId, ChildCount++);
        }
    }
}
