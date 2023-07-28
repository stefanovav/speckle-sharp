#include "ArchicadBrowserBridge.hpp"


DUI3::ArchicadBrowserBridge::ArchicadBrowserBridge(void* browser, IBinding* binding/*, Action<string> executeScriptAsync, Action showDevToolsAction*/)
{
	/*
	FrontendBoundName = binding.Name;
	Browser = browser;
	Binding = binding;

	BindingType = Binding.GetType();
	BindingMethodCache = new Dictionary<string, MethodInfo>();
	// Note: we need to filter out getter and setter methods here because they are not really nicely
	// supported across browsers, hence the !method.IsSpecialName.
	foreach(var m in BindingType.GetMethods().Where(method => !method.IsSpecialName))
	{
	BindingMethodCache[m.Name] = m;
	}

	Binding.Parent = this;

	ExecuteScriptAsync = executeScriptAsync;
	ShowDevToolsAction = showDevToolsAction;

	serializerOptions = new JsonSerializerOptions
	{
	PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};*/
}


GS::UniString DUI3::ArchicadBrowserBridge::RunMethod(GS::String methodName, GS::UniString args)
{
	/*
	// Note: You might be tempted to make this method async Task<string> to prevent the task.Wait() below.
	// Do not do that! Cef65 doesn't like waiting for async .NET methods.
	// Note: we have this pokemon catch 'em all here because throwing errors in .NET is
	// very risky, and we might crash the host application. Behaviour seems also to differ
	// between various browser controls (e.g.: cefsharp handles things nicely - basically
	// passing back the exception to the browser, but webview throws an access violation
	// error that kills Rhino.).
	try
	{
	if (!BindingMethodCache.ContainsKey(methodName))
	  throw new SpeckleException($"Cannot find method {methodName} in bindings class {BindingType.AssemblyQualifiedName}.");

	var method = BindingMethodCache[methodName];
	var parameters = method.GetParameters();
	var jsonArgsArray = JsonSerializer.Deserialize<string[]>(args);

	if (parameters.Length != jsonArgsArray.Length)
	  throw new SpeckleException($"Wrong number of arguments when invoking binding function {methodName}, expected {parameters.Length}, but got {jsonArgsArray.Length}.");

	var typedArgs = new object[jsonArgsArray.Length];

	for (int i = 0; i < typedArgs.Length; i++)
	{
	  var typedObj = JsonSerializer.Deserialize(jsonArgsArray[i], parameters[i].ParameterType);
	  typedArgs[i] = typedObj;
	}
	var resultTyped = method.Invoke(Binding, typedArgs);

	// Was it an async method (in bridgeClass?)
	var resultTypedTask = resultTyped as Task;

	string resultJson;

	// Was the method called async?
	if (resultTypedTask == null)
	{
	  // Regular method: no need to await things
	  resultJson = JsonSerializer.Serialize(resultTyped, serializerOptions);
	}
	else // It's an async call
	{
	  // See note at start of function. Do not asyncify!
	  resultTypedTask.Wait();

	  // If has a "Result" property return the value otherwise null (Task<void> etc)
	  var resultProperty = resultTypedTask.GetType().GetProperty("Result");
	  var taskResult = resultProperty != null ? resultProperty.GetValue(resultTypedTask) : null;
	  resultJson = JsonSerializer.Serialize(taskResult, serializerOptions);
	}

	return resultJson;
	}
	catch (Exception e)
	{
	// TODO: properly log the exeception.
	return JsonSerializer.Serialize(new { Error = e.Message, InnerError = e.InnerException?.Message }, serializerOptions);
	}*/
	
	return "";
}


void DUI3::ArchicadBrowserBridge::SendToBrowser (GS::String eventName, void* data /*= nullptr*/)
{
	/*string script;
	if (data != null)
	{
	var payload = JsonSerializer.Serialize(data, serializerOptions);
	script = $"{FrontendBoundName}.emit('{eventName}', '{payload}')";
	}
	else
	{
	script = $"{FrontendBoundName}.emit('{eventName}')";
	}
	ExecuteScriptAsync(script);*/
}

void DUI3::ArchicadBrowserBridge::ShowDevTools()
{
	//ShowDevToolsAction();
}
