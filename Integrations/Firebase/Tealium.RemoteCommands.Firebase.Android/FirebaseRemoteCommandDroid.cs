using System;
using System.Collections.Generic;
using Tealium;
using Android.App;
using Android.OS;
using Firebase.Analytics;

namespace Tealium.RemoteCommands.Firebase.Droid
{

    public class FirebaseRemoteCommandDroid : FirebaseRemoteCommand, IRemoteCommand
    {
        private static Activity currentActivity;

        private static FirebaseAnalytics firebaseAnalytics;

        private string path;
        private string url;

        public override string Path => path;

        public override string Url => url;

        static FirebaseRemoteCommandDroid()
        {
            eventsMap.Add("event_ad_impression", FirebaseAnalytics.Event.AdImpression);
            eventsMap.Add("event_add_shipping_info", FirebaseAnalytics.Event.AddShippingInfo);
            eventsMap.Add("event_add_payment_info", FirebaseAnalytics.Event.AddPaymentInfo);
            eventsMap.Add("event_add_to_cart", FirebaseAnalytics.Event.AddToCart);
            eventsMap.Add("event_add_to_wishlist", FirebaseAnalytics.Event.AddToWishlist);
            eventsMap.Add("event_app_open", FirebaseAnalytics.Event.AppOpen);
            eventsMap.Add("event_begin_checkout", FirebaseAnalytics.Event.BeginCheckout);
            eventsMap.Add("event_campaign_details", FirebaseAnalytics.Event.CampaignDetails);
            eventsMap.Add("event_checkout_progress", FirebaseAnalytics.Event.CheckoutProgress);
            eventsMap.Add("event_earn_virtual_currency", FirebaseAnalytics.Event.EarnVirtualCurrency);
            eventsMap.Add("event_ecommerce_purchase", FirebaseAnalytics.Event.EcommercePurchase);
            eventsMap.Add("event_generate_lead", FirebaseAnalytics.Event.GenerateLead);
            eventsMap.Add("event_join_group", FirebaseAnalytics.Event.JoinGroup);
            eventsMap.Add("event_level_start", FirebaseAnalytics.Event.LevelStart);
            eventsMap.Add("event_level_end", FirebaseAnalytics.Event.LevelEnd);
            eventsMap.Add("event_level_up", FirebaseAnalytics.Event.LevelUp);
            eventsMap.Add("event_login", FirebaseAnalytics.Event.Login);
            eventsMap.Add("event_post_score", FirebaseAnalytics.Event.PostScore);
            eventsMap.Add("event_present_offer", FirebaseAnalytics.Event.PresentOffer);
            eventsMap.Add("event_purchase", FirebaseAnalytics.Event.Purchase);
            eventsMap.Add("event_purchase_refund", FirebaseAnalytics.Event.PurchaseRefund);
            eventsMap.Add("event_remove_cart", FirebaseAnalytics.Event.RemoveFromCart);
            eventsMap.Add("event_refund", FirebaseAnalytics.Event.Refund);
            eventsMap.Add("event_screen_view", FirebaseAnalytics.Event.ScreenView);
            eventsMap.Add("event_search", FirebaseAnalytics.Event.Search);
            eventsMap.Add("event_select_content", FirebaseAnalytics.Event.SelectContent);
            eventsMap.Add("event_select_item", FirebaseAnalytics.Event.SelectItem);
            eventsMap.Add("event_select_promotion", FirebaseAnalytics.Event.SelectPromotion);
            eventsMap.Add("event_set_checkout_option", FirebaseAnalytics.Event.SetCheckoutOption);
            eventsMap.Add("event_share", FirebaseAnalytics.Event.Share);
            eventsMap.Add("event_signup", FirebaseAnalytics.Event.SignUp);
            eventsMap.Add("event_spend_virtual_currency", FirebaseAnalytics.Event.SpendVirtualCurrency);
            eventsMap.Add("event_tutorial_begin", FirebaseAnalytics.Event.TutorialBegin);
            eventsMap.Add("event_tutorial_complete", FirebaseAnalytics.Event.TutorialComplete);
            eventsMap.Add("event_unlock_achievement", FirebaseAnalytics.Event.UnlockAchievement);
            eventsMap.Add("event_view_cart", FirebaseAnalytics.Event.ViewCart);
            eventsMap.Add("event_view_item", FirebaseAnalytics.Event.ViewItem);
            eventsMap.Add("event_view_item_list", FirebaseAnalytics.Event.ViewItemList);
            eventsMap.Add("event_view_promotion", FirebaseAnalytics.Event.ViewPromotion);
            eventsMap.Add("event_view_search_results", FirebaseAnalytics.Event.ViewSearchResults);


            parameters.Add("param_achievement_id", FirebaseAnalytics.Param.AchievementId);
            parameters.Add("param_ad_format", FirebaseAnalytics.Param.AdFormat);
            parameters.Add("param_ad_network_click_id", FirebaseAnalytics.Param.Aclid);
            parameters.Add("param_ad_platform", FirebaseAnalytics.Param.AdPlatform);
            parameters.Add("param_ad_source", FirebaseAnalytics.Param.AdSource);
            parameters.Add("param_ad_unit_name", FirebaseAnalytics.Param.AdUnitName);
            parameters.Add("param_affiliation", FirebaseAnalytics.Param.Affiliation);
            parameters.Add("param_cp1", FirebaseAnalytics.Param.Cp1);
            parameters.Add("param_campaign", FirebaseAnalytics.Param.Campaign);
            parameters.Add("param_character", FirebaseAnalytics.Param.Character);
            parameters.Add("param_checkout_option", FirebaseAnalytics.Param.CheckoutOption);
            parameters.Add("param_checkout_step", FirebaseAnalytics.Param.CheckoutStep);
            parameters.Add("param_content", FirebaseAnalytics.Param.Content);
            parameters.Add("param_content_type", FirebaseAnalytics.Param.ContentType);
            parameters.Add("param_coupon", FirebaseAnalytics.Param.Coupon);
            parameters.Add("param_creative_name", FirebaseAnalytics.Param.CreativeName);
            parameters.Add("param_creative_slot", FirebaseAnalytics.Param.CreativeSlot);
            parameters.Add("param_currency", FirebaseAnalytics.Param.Currency);
            parameters.Add("param_destination", FirebaseAnalytics.Param.Destination);
            parameters.Add("param_discount", FirebaseAnalytics.Param.Discount);
            parameters.Add("param_end_date", FirebaseAnalytics.Param.EndDate);
            parameters.Add("param_extend_session", FirebaseAnalytics.Param.ExtendSession);
            parameters.Add("param_flight_number", FirebaseAnalytics.Param.FlightNumber);
            parameters.Add("param_group_id", FirebaseAnalytics.Param.GroupId);
            parameters.Add("param_index", FirebaseAnalytics.Param.Index);
            parameters.Add("param_item_brand", FirebaseAnalytics.Param.ItemBrand);
            parameters.Add("param_item_category", FirebaseAnalytics.Param.ItemCategory);
            parameters.Add("param_item_category2", FirebaseAnalytics.Param.ItemCategory2);
            parameters.Add("param_item_category3", FirebaseAnalytics.Param.ItemCategory3);
            parameters.Add("param_item_category4", FirebaseAnalytics.Param.ItemCategory4);
            parameters.Add("param_item_category5", FirebaseAnalytics.Param.ItemCategory5);
            parameters.Add("param_item_id", FirebaseAnalytics.Param.ItemId);
            parameters.Add("param_item_list", FirebaseAnalytics.Param.ItemList);
            parameters.Add("param_item_list_id", FirebaseAnalytics.Param.ItemListId);
            parameters.Add("param_item_list_name", FirebaseAnalytics.Param.ItemListName);
            parameters.Add("param_item_location_id", FirebaseAnalytics.Param.ItemLocationId);
            parameters.Add("param_item_name", FirebaseAnalytics.Param.ItemName);
            parameters.Add("param_item_variant", FirebaseAnalytics.Param.ItemVariant);
            parameters.Add("param_items", FirebaseAnalytics.Param.Items);
            parameters.Add("param_level", FirebaseAnalytics.Param.Level);
            parameters.Add("param_level_name", FirebaseAnalytics.Param.LevelName);
            parameters.Add("param_location", FirebaseAnalytics.Param.Location);
            parameters.Add("param_location_id", FirebaseAnalytics.Param.LocationId);
            parameters.Add("param_medium", FirebaseAnalytics.Param.Medium);
            parameters.Add("param_number_nights", FirebaseAnalytics.Param.NumberOfNights);
            parameters.Add("param_number_pax", FirebaseAnalytics.Param.NumberOfPassengers);
            parameters.Add("param_number_rooms", FirebaseAnalytics.Param.NumberOfRooms);
            parameters.Add("param_origin", FirebaseAnalytics.Param.Origin);
            parameters.Add("param_payment_type", FirebaseAnalytics.Param.PaymentType);
            parameters.Add("param_price", FirebaseAnalytics.Param.Price);
            parameters.Add("param_promotion_id", FirebaseAnalytics.Param.PromotionId);
            parameters.Add("param_promotion_name", FirebaseAnalytics.Param.PromotionName);
            parameters.Add("param_quantity", FirebaseAnalytics.Param.Quantity);
            parameters.Add("param_score", FirebaseAnalytics.Param.Score);
            parameters.Add("param_screen_class", FirebaseAnalytics.Param.ScreenClass);
            parameters.Add("param_screen_name", FirebaseAnalytics.Param.ScreenName);
            parameters.Add("param_search_term", FirebaseAnalytics.Param.SearchTerm);
            parameters.Add("param_shipping", FirebaseAnalytics.Param.Shipping);
            parameters.Add("param_shipping_tier", FirebaseAnalytics.Param.ShippingTier);
            parameters.Add("param_signup_method", FirebaseAnalytics.Param.SignUpMethod);
            parameters.Add("param_source", FirebaseAnalytics.Param.Source);
            parameters.Add("param_start_date", FirebaseAnalytics.Param.StartDate);
            parameters.Add("param_success", FirebaseAnalytics.Param.Success);
            parameters.Add("param_tax", FirebaseAnalytics.Param.Tax);
            parameters.Add("param_term", FirebaseAnalytics.Param.Term);
            parameters.Add("param_transaction_id", FirebaseAnalytics.Param.TransactionId);
            parameters.Add("param_travel_class", FirebaseAnalytics.Param.TravelClass);
            parameters.Add("param_value", FirebaseAnalytics.Param.Value);
            parameters.Add("param_virtual_currency_name", FirebaseAnalytics.Param.VirtualCurrencyName);
            parameters.Add("param_user_signup_method", FirebaseAnalytics.UserProperty.SignUpMethod);
            parameters.Add("param_user_allow_ad_personalization_signals", FirebaseAnalytics.UserProperty.AllowAdPersonalizationSignals);

        }


        public FirebaseRemoteCommandDroid(Application app, string path, string url) : base()
        {
            this.path = path;
            this.url = url;
            firebaseAnalytics = FirebaseAnalytics.GetInstance(app.ApplicationContext);
            Application.IActivityLifecycleCallbacks cb = new LifecycleCallbacks();

            app.RegisterActivityLifecycleCallbacks(cb);
        }


        protected override void Configure(int? sessionTimeoutDuration, bool? analyticsEnabled, string loggerLevel)
        {
            if (sessionTimeoutDuration.HasValue)
            {
                firebaseAnalytics?.SetSessionTimeoutDuration(sessionTimeoutDuration.Value);
            }

            if (analyticsEnabled.HasValue)
            {
                firebaseAnalytics?.SetAnalyticsCollectionEnabled(analyticsEnabled.Value);
            }

        }

        private Bundle JSONToBundle(Dictionary<string, object> eventParams)
        {
            Bundle bundle = new Bundle();

            foreach (var key in eventParams.Keys)
            {
                switch (key)
                {
                    case FirebaseAnalytics.Param.Discount:
                    case FirebaseAnalytics.Param.Price:
                    case FirebaseAnalytics.Param.Shipping:
                    case FirebaseAnalytics.Param.Tax:
                    case FirebaseAnalytics.Param.Value:
                        bundle.PutDouble(key, Convert.ToDouble(eventParams[key]));
                        break;
                    case FirebaseAnalytics.Param.Level:
                    case FirebaseAnalytics.Param.NumberOfNights:
                    case FirebaseAnalytics.Param.NumberOfPassengers:
                    case FirebaseAnalytics.Param.NumberOfRooms:
                    case FirebaseAnalytics.Param.Quantity:
                    case FirebaseAnalytics.Param.Score:
                    case FirebaseAnalytics.Param.Success:
                        bundle.PutLong(key, Convert.ToInt64(eventParams[key]));
                        break;
                    case FirebaseAnalytics.Param.Items:
                        Dictionary<string, object>[] items = (Dictionary<string,object>[])eventParams[key];
                        IParcelable[] itemList = new IParcelable[items.Length];
                        for (int i = 0; i < items.Length; i++)
                        {
                            itemList[i] = JSONToBundle(items[i]);
                        }
                        bundle.PutParcelableArray(key, itemList);
                        break;
                    default:
                        bundle.PutString(key, eventParams[key].ToString());
                        break;
                }
            }
            return bundle;
        }

        protected override void LogEvent(string eventName, Dictionary<string, object> eventParams)
        {
            if (eventName != null && eventParams != null)
            {
                firebaseAnalytics?.LogEvent(eventName, JSONToBundle(eventParams));
            }
        }

        protected override void SetScreenName(string screenName, string screenClass)
        {
            LogEvent(FirebaseAnalytics.Event.ScreenView, new Dictionary<string, object>
            {
                {FirebaseAnalytics.Param.ScreenName, screenName },
                {FirebaseAnalytics.Param.ScreenClass, screenClass }
            });
        }

        protected override void SetUserProperty(string propertyName, string propertyValue)
        {
            firebaseAnalytics?.SetUserProperty(propertyName, propertyValue);
        }

        protected override void SetUserId(string userId)
        {
            firebaseAnalytics?.SetUserId(userId);
        }

        // Setup lifecycle callbacks to init FirebaseAnalytics. You may prefer to do this manually.

        private class LifecycleCallbacks : Java.Lang.Object, Application.IActivityLifecycleCallbacks
        {
            //public IntPtr Handle => throw new NotImplementedException();
            public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
            {
                currentActivity = activity;
                firebaseAnalytics = FirebaseAnalytics.GetInstance(currentActivity.ApplicationContext);
            }

            public void OnActivityDestroyed(Activity activity)
            {

            }

            public void OnActivityPaused(Activity activity)
            {

            }

            public void OnActivityResumed(Activity activity)
            {

            }

            public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
            {

            }

            public void OnActivityStarted(Activity activity)
            {

            }

            public void OnActivityStopped(Activity activity)
            {

            }
        }

    }

}