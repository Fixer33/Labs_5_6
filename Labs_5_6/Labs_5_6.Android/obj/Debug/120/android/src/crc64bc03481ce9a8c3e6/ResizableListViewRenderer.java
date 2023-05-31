package crc64bc03481ce9a8c3e6;


public class ResizableListViewRenderer
	extends crc643f46942d9dd1fff9.ListViewRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Labs_5_6.Droid.ResizableListViewRenderer, Labs_5_6.Android", ResizableListViewRenderer.class, __md_methods);
	}


	public ResizableListViewRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ResizableListViewRenderer.class)
			mono.android.TypeManager.Activate ("Labs_5_6.Droid.ResizableListViewRenderer, Labs_5_6.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ResizableListViewRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ResizableListViewRenderer.class)
			mono.android.TypeManager.Activate ("Labs_5_6.Droid.ResizableListViewRenderer, Labs_5_6.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public ResizableListViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ResizableListViewRenderer.class)
			mono.android.TypeManager.Activate ("Labs_5_6.Droid.ResizableListViewRenderer, Labs_5_6.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
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
