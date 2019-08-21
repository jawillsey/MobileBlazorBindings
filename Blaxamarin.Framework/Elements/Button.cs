﻿using Emblazon;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Xamarin.Forms;

namespace Blaxamarin.Framework.Elements
{
    public class Button : FormsComponentBase
    {
        static Button()
        {
            BlelementAdapter.KnownElements.Add(typeof(Button).FullName, new ComponentControlFactoryFunc<Element>((renderer, _) => new BlazorButton(renderer)));
        }

        [Parameter] public string Text { get; set; }
        [Parameter] public EventCallback OnClick { get; set; }

        protected override void RenderAttributes(RenderTreeBuilder builder)
        {
            if (Text != null)
            {
                builder.AddAttribute(1, nameof(Text), Text);
            }
            builder.AddAttribute(2, "onclick", OnClick);
        }

        class BlazorButton : Xamarin.Forms.Button, IBlazorNativeControl
        {
            public BlazorButton(EmblazonRenderer<Element> renderer)
            {
                Clicked += (s, e) =>
                {
                    if (ClickEventHandlerId != default)
                    {
                        renderer.Dispatcher.InvokeAsync(() => renderer.DispatchEventAsync(ClickEventHandlerId, null, new UIEventArgs()));
                    }
                };
            }

            public ulong ClickEventHandlerId { get; set; }

            public void ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName)
            {
                switch (attributeName)
                {
                    case nameof(Text):
                        Text = (string)attributeValue;
                        break;
                    case "onclick":
                        ClickEventHandlerId = attributeEventHandlerId;
                        break;
                    default:
                        FormsComponentBase.ApplyAttribute(this, attributeEventHandlerId, attributeName, attributeValue, attributeEventUpdatesAttributeName);
                        break;
                }
            }
        }

    }
}
