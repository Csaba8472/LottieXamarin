﻿using System;
using System.ComponentModel;
using Com.Airbnb.Lottie;
using Lottie.Forms;
using Lottie.Forms.Platforms.Android;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer)), Preserve(AllMembers = true)]
namespace Lottie.Forms.Platforms.Android
{
#pragma warning disable 0618
    public class AnimationViewRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<AnimationView,
        LottieAnimationView>
    {
        private LottieAnimationView _animationView;
        private AnimatorListener _animatorListener;
        private AnimatorUpdateListener _animatorUpdateListener;
        private LottieOnCompositionLoadedListener _lottieOnCompositionLoadedListener;
        private LottieFailureListener _lottieFailureListener;
        private ClickListener _clickListener;

        protected override void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            base.OnElementChanged(e);

            if (e == null)
                return;

            if (e.OldElement != null)
            {
                _animationView.RemoveAnimatorListener(_animatorListener);
                _animationView.RemoveAllUpdateListeners();
                _animationView.RemoveLottieOnCompositionLoadedListener(_lottieOnCompositionLoadedListener);
                _animationView.SetFailureListener(null);
                _animationView.SetOnClickListener(null);
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _animationView = new LottieAnimationView(Context);
                    _animatorListener = new AnimatorListener
                    {
                        OnAnimationCancelImpl = () => e.NewElement.InvokeStopAnimation(),
                        OnAnimationEndImpl = () => e.NewElement.InvokeFinishedAnimation(),
                        OnAnimationPauseImpl = () => e.NewElement.InvokePauseAnimation(),
                        OnAnimationRepeatImpl = () => e.NewElement.InvokeRepeatAnimation(),
                        OnAnimationResumeImpl = () => e.NewElement.InvokeResumeAnimation(),
                        OnAnimationStartImpl = () => e.NewElement.InvokePlayAnimation()
                    };
                    _animatorUpdateListener = new AnimatorUpdateListener
                    {
                        OnAnimationUpdateImpl = (progress) => e.NewElement.InvokeAnimationUpdate(progress)
                    };
                    _lottieOnCompositionLoadedListener = new LottieOnCompositionLoadedListener
                    {
                        OnCompositionLoadedImpl = (composition) => e.NewElement.InvokeAnimationLoaded(composition)
                    };
                    _lottieFailureListener = new LottieFailureListener
                    {
                        OnResultImpl = (exception) => e.NewElement.InvokeFailure(exception)
                    };
                    _clickListener = new ClickListener
                    {
                        OnClickImpl = () => e.NewElement.InvokeClick()
                    };

                    _animationView.AddAnimatorListener(_animatorListener);
                    _animationView.AddAnimatorUpdateListener(_animatorUpdateListener);
                    _animationView.AddLottieOnCompositionLoadedListener(_lottieOnCompositionLoadedListener);
                    _animationView.SetFailureListener(_lottieFailureListener);
                    _animationView.SetOnClickListener(_clickListener);

                    _animationView.TrySetAnimation(e.NewElement);

                    e.NewElement.PlayCommand = new Command(() => _animationView.PlayAnimation());
                    e.NewElement.PauseCommand = new Command(() => _animationView.PauseAnimation());
                    e.NewElement.ResumeCommand = new Command(() => _animationView.ResumeAnimation());
                    e.NewElement.StopCommand = new Command(() => _animationView.CancelAnimation());
                    e.NewElement.ClickCommand = new Command(() => _animationView.PerformClick());

                    e.NewElement.PlayMinAndMaxFrameCommand = new Command((object paramter) =>
                    {
                        if (paramter is (int minFrame, int maxFrame))
                        {
                            _animationView.SetMinAndMaxFrame(minFrame, maxFrame);
                            _animationView.PlayAnimation();
                        }
                    });
                    e.NewElement.PlayMinAndMaxProgressCommand = new Command((object paramter) =>
                    {
                        if (paramter is (float minProgress, float maxProgress))
                        {
                            _animationView.SetMinAndMaxProgress(minProgress, maxProgress);
                            _animationView.PlayAnimation();
                        }
                    });
                    e.NewElement.ReverseAnimationSpeedCommand = new Command(() => _animationView.ReverseAnimationSpeed());

                    _animationView.SetCacheComposition(e.NewElement.CacheComposition);
                    //_animationView.SetFallbackResource(e.NewElement.FallbackResource.);
                    //_animationView.Composition = e.NewElement.Composition;

                    if (e.NewElement.MinFrame != int.MinValue)
                        _animationView.SetMinFrame(e.NewElement.MinFrame);
                    if (e.NewElement.MinProgress != float.MinValue)
                        _animationView.SetMinProgress(e.NewElement.MinProgress);
                    if (e.NewElement.MaxFrame != int.MinValue)
                        _animationView.SetMaxFrame(e.NewElement.MaxFrame);
                    if (e.NewElement.MaxProgress != float.MinValue)
                        _animationView.SetMaxProgress(e.NewElement.MaxProgress);

                    _animationView.Speed = e.NewElement.Speed;

                    _animationView.ConfigureRepeat(e.NewElement.RepeatMode, e.NewElement.RepeatCount);

                    if (!string.IsNullOrEmpty(e.NewElement.ImageAssetsFolder))
                        _animationView.ImageAssetsFolder = e.NewElement.ImageAssetsFolder;
                    _animationView.Scale = e.NewElement.Scale;
                    _animationView.Frame = e.NewElement.Frame;
                    _animationView.Progress = e.NewElement.Progress;

                    SetNativeControl(_animationView);

                    if (e.NewElement.AutoPlay || e.NewElement.IsAnimating)
                        _animationView.PlayAnimation();

                    e.NewElement.Duration = _animationView.Duration;
                    e.NewElement.IsAnimating = _animationView.IsAnimating;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_animationView == null || Element == null || e == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                _animationView.TrySetAnimation(Element);

                if (Element.AutoPlay || Element.IsAnimating)
                    _animationView.PlayAnimation();
            }

            //if (e.PropertyName == AnimationView.AutoPlayProperty.PropertyName)
            //    _animationView.AutoPlay = (Element.AutoPlay);

            if (e.PropertyName == AnimationView.CacheCompositionProperty.PropertyName)
                _animationView.SetCacheComposition(Element.CacheComposition);

            //if (e.PropertyName == AnimationView.FallbackResource.PropertyName)
            //    _animationView.SetFallbackResource(e.NewElement.FallbackResource);

            //if (e.PropertyName == AnimationView.Composition.PropertyName)
            //    _animationView.Composition = e.NewElement.Composition;

            if (e.PropertyName == AnimationView.MinFrameProperty.PropertyName)
                _animationView.SetMinFrame(Element.MinFrame);

            if (e.PropertyName == AnimationView.MinProgressProperty.PropertyName)
                _animationView.SetMinProgress(Element.MinProgress);

            if (e.PropertyName == AnimationView.MaxFrameProperty.PropertyName)
                _animationView.SetMaxFrame(Element.MaxFrame);

            if (e.PropertyName == AnimationView.MaxProgressProperty.PropertyName)
                _animationView.SetMaxProgress(Element.MaxProgress);

            if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
                _animationView.Speed = Element.Speed;

            if (e.PropertyName == AnimationView.RepeatModeProperty.PropertyName || e.PropertyName == AnimationView.RepeatCountProperty.PropertyName)
                _animationView.ConfigureRepeat(Element.RepeatMode, Element.RepeatCount);

            if (e.PropertyName == AnimationView.ImageAssetsFolderProperty.PropertyName && !string.IsNullOrEmpty(Element.ImageAssetsFolder))
                _animationView.ImageAssetsFolder = Element.ImageAssetsFolder;

            if (e.PropertyName == AnimationView.ScaleProperty.PropertyName)
                _animationView.Scale = Element.Scale;

            if (e.PropertyName == AnimationView.FrameProperty.PropertyName)
                _animationView.Frame = Element.Frame;

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
                _animationView.Progress = Element.Progress;

            base.OnElementPropertyChanged(sender, e);
        }
    }
#pragma warning restore 0618
}
