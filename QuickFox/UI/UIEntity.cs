using System;
using System.Collections.Generic;
using System.Drawing;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.Rendering;

namespace QuickFox.UI
{
    public class UIEntity : Entity
    {
        protected readonly IEntityManager EntityManager;

        public string LayerId { get; set; }

        public int WidthRequest { get; set; } = LayoutConstants.WrapContent;

        public int HeightRequest { get; set; } = LayoutConstants.WrapContent;

        public int MarginLeft { get; set; } = 0;

        public int MarginRight { get; set; } = 0;

        public int MarginTop { get; set; } = 0;

        public int MarginBottom { get; set; } = 0;

        public HorizontalAlignment HorizontalAlignment { get; set; }

        public VerticalAlignment VerticalAlignment { get; set; }

        public UIBounds Bounds { get; set; }

        public TilingMode BackgroundTilingMode { get; set; }

        public string Tag { get; set; }

        public string ParentId { get; set; }

        internal int OrderInParent { get; set; }

        public UIEntity(IEntityManager entityManager)
        {
            EntityManager = entityManager;
            Bounds = new UIBounds();
        }

        public UIEntity ParentView => GetParentView();

        protected UIEntity GetParentView()
        {
            if (string.IsNullOrWhiteSpace(ParentId))
                return null;

            return EntityManager.GetEntity(ParentId) as UIEntity;
        }

        internal virtual void Invalidate()
        {
            Bounds.IsValid = false;

            if (ParentView?.Bounds.IsValid ?? false)
            {
                ParentView?.Invalidate();
            }
        }

        internal virtual void InvalidateInParent()
        {
            Bounds.IsValid = false;
        }

        internal virtual void Measure(int availableWidth, int availableHeight)
        {
            Bounds.Width = WidthRequest == LayoutConstants.MatchParent ? availableWidth : Math.Min(availableWidth, WidthRequest);
            Bounds.Height = HeightRequest == LayoutConstants.MatchParent ? availableHeight : Math.Min(availableHeight, HeightRequest);
        }

        internal virtual void Layout(int newX, int newY)
        {
            Bounds.IsValid = true;
            Bounds.X = newX;
            Bounds.Y = newY;
        }
    }
}
