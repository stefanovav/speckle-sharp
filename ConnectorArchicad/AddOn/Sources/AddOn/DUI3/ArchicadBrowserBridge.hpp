#ifndef DUI3_BRIDGE_HPP
#define DUI3_BRIDGE_HPP

#include "Interfaces.hpp"

#include "HashTable.hpp"


namespace DUI3
{

/// <summary>
/// Wraps a binding class, and manages its calls from the Frontend to .NET, and sending events from .NET to the the Frontend.
/// <para>See also: https://github.com/johot/WebView2-better-bridge</para>
/// </summary>
class ArchicadBrowserBridge : IBridge
{
public:
	/// <summary>
	/// The name under which we expect the frontend to hoist this bindings class to the global scope.
	/// e.g., `receiveBindings` should be available as `window.receiveBindings`.
	/// </summary>
	GS::UniString GetFrontendBoundName ();

	void* GetBrowser ();

	IBinding* GetBinding ();

	//public Action<string> ExecuteScriptAsync { get; set; }
	//public Action ShowDevToolsAction { get; set; }

	//private Type BindingType { get; set; }
	GS::HashTable<GS::String, GS::String> bindingMethodCache;

	//private JsonSerializerOptions serializerOptions;

	/// <summary>
	/// Creates a new bridge.
	/// </summary>
	/// <param name="browser">The host browser instance.</param>
	/// <param name="binding">The actual binding class.</param>
	/// <param name="executeScriptAsync">A simple action that does the browser's version of executeScriptAsync(string).</param>
	ArchicadBrowserBridge(void* browser, IBinding* binding/*, Action<string> executeScriptAsync, Action showDevToolsAction*/);

	/// <summary>
	/// Used by the Frontend bridge logic to understand which methods are available.
	/// </summary>
	/// <returns></returns>
	GS::Array<GS::String> GetBindingMethodCache ();

	/// <summary>
	/// Used by the Frontend bridge to call into .NET.
	/// TODO: Check and test
	/// </summary>
	/// <param name="methodName"></param>
	/// <param name="args"></param>
	/// <returns></returns>
	GS::UniString RunMethod(GS::String methodName, GS::UniString args);

	/// <summary>
	/// Notifies the Frontend about something by doing the browser specific way for `browser.ExecuteScriptAsync("window.FrontendBoundName.on(eventName, etc.)")`.
	/// </summary>
	/// <param name="eventData"></param>
	void SendToBrowser (GS::String eventName, void* data = nullptr);

	/// <summary>
	/// Shows the dev tools. This is currently only needed for CefSharp - other browser
	/// controls allow for right click + inspect.
	/// </summary>
	void ShowDevTools ();
};


}

#endif
