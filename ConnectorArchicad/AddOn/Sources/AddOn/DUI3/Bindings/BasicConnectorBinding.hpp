#ifndef DUI3_BASICCONNECTORBINDING_HPP
#define DUI3_BASICCONNECTORBINDING_HPP

#include "Interfaces.hpp"


namespace DUI3
{

public class IBasicConnectorBinding : IBinding
{
	public string GetSourceApplicationName();
	public string GetSourceApplicationVersion();
	public Account[] GetAccounts();
	public DocumentInfo GetDocumentInfo();
}

public static class BasicConnectorBindingEvents
{
	public static readonly string DisplayToastNotification = "DisplayToastNotification";
	public static readonly string DocumentChanged = "documentChanged";
}

}
