package crc64a17eccbbc6018201;


public class DatePickerCustomRenderAndroid
	extends crc643f46942d9dd1fff9.DatePickerRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("App.Droid.DatePickerCustomRenderAndroid, App.Android", DatePickerCustomRenderAndroid.class, __md_methods);
	}


	public DatePickerCustomRenderAndroid (android.content.Context p0)
	{
		super (p0);
		if (getClass () == DatePickerCustomRenderAndroid.class) {
			mono.android.TypeManager.Activate ("App.Droid.DatePickerCustomRenderAndroid, App.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public DatePickerCustomRenderAndroid (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == DatePickerCustomRenderAndroid.class) {
			mono.android.TypeManager.Activate ("App.Droid.DatePickerCustomRenderAndroid, App.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public DatePickerCustomRenderAndroid (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == DatePickerCustomRenderAndroid.class) {
			mono.android.TypeManager.Activate ("App.Droid.DatePickerCustomRenderAndroid, App.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
