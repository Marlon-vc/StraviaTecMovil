using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StraviaTecMovil.Helpers
{
    public static class Messaging
    {
        public static void Subscribe<TSender>(object subscriber, string key, Action<TSender> callback)
            where TSender : class
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Subscribe(subscriber, key, callback);
            });
        }

        public static void Subscribe<TSender, TPayload>(object subscriber, string key, Action<TSender, TPayload> callback)
            where TSender : class
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Subscribe(subscriber, key, callback);
            });
        }

        public static void Unsubscribe<TSender>(object subscriber, string key)
            where TSender : class
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Unsubscribe<TSender>(subscriber, key);
            });
        }

        public static void Send<TSender>(TSender sender, string key)
            where TSender : class
        {
            Device.BeginInvokeOnMainThread(() => {
                MessagingCenter.Send(sender, key);
            });
        }

        public static void Send<TSender, TPayload>(TSender sender, string key, TPayload payload)
            where TSender : class
        {
            Device.BeginInvokeOnMainThread(() => {
                MessagingCenter.Send(sender, key, payload);
            });
        }
    }
}
