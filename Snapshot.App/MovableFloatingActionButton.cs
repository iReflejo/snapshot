using Android.Content;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Java.Lang;

public class MovableFloatingActionButton : FloatingActionButton, View.IOnTouchListener
{

    private const int ClickDragTolerance = 10; // Often, there will be a slight, unintentional, drag when the user taps the FAB, so we need to account for this.

    private float _downRawX;
    private float _downRawY;
    private float _dX;
    private float _dY;

    public MovableFloatingActionButton(Context context) : base(context)
    {
        Init();
    }

    public MovableFloatingActionButton(Context context, IAttributeSet attrs) : base(context, attrs)
    {
        Init();
    }

    public MovableFloatingActionButton(Context context, IAttributeSet attrs, int defStyleAttr): base(context, attrs, defStyleAttr)
    {
        Init();
    }

    private void Init()
    {
        this.SetOnTouchListener(this);
    }
    
    public bool OnTouch(View view, MotionEvent motionEvent)
    {
        var action = motionEvent.Action;
        switch (action)
        {
            case MotionEventActions.Down:
                _downRawX = motionEvent.RawX;
                _downRawY = motionEvent.RawY;
                _dX = view.GetX() - _downRawX;
                _dY = view.GetY() - _downRawY;

                return true; // Consumed
            case MotionEventActions.Move:
            {
                var viewWidth = view.Width;
                var viewHeight = view.Height;
                    
                var viewParent = (View)view.Parent;
                var parentWidth = viewParent.Width;
                var parentHeight = viewParent.Height;
                
                var newX = motionEvent.RawX + _dX;
                newX = Math.Max(0, newX); // Don't allow the FAB past the left hand side of the parent
                newX = Math.Min(parentWidth - viewWidth, newX); // Don't allow the FAB past the right hand side of the parent

                var newY = motionEvent.RawY + _dY;
                newY = Math.Max(0, newY); // Don't allow the FAB past the top of the parent
                newY = Math.Min(parentHeight - viewHeight, newY); // Don't allow the FAB past the bottom of the parent

                view.Animate()
                    .X(newX)
                    .Y(newY)
                    .SetDuration(0)
                    .Start();

                return true; // Consumed
            }
            case MotionEventActions.Up:
            {
                var upRawX = motionEvent.GetX();
                var upRawY = motionEvent.GetY();

                var upDx = upRawX - _downRawX;
                var upDy = upRawY - _downRawY;

                if (Math.Abs(upDx) < ClickDragTolerance && Math.Abs(upDy) < ClickDragTolerance)
                { // A click
                    return PerformClick();
                }
                else
                { // A drag
                    return true; // Consumed
                }
            }
            default:
                return base.OnTouchEvent(motionEvent);
        }
    }
}