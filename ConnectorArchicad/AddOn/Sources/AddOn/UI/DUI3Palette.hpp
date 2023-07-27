#ifndef DUI3_PALETTE_HPP
#define DUI3_PALETTE_HPP

#include "ResourceIds.hpp"

// DG
#include "DGBrowser.hpp"
#include "DGDialog.hpp"
#include "DGEditControl.hpp"


namespace DG {
	class ShortcutScheme;
}


namespace DUI3 {


class Palette final : public DG::Palette,
					  private DG::PanelObserver
{
private:
	enum {
		PaletteResourceId = ID_DUI3_PALETTE,
		BrowserId		= 1
	};

	enum {
		PaletteMenuResourceId = ID_ADDON_MENU,
		PaletteMenuItemId = 2
	};

private:
	static GS::Guid paletteGuid;

	static GS::Ref<DUI3::Palette> instance;

	DG::Browser		browser;

	short			enterHotKey;
	short			returnHotKey;
	
	DG::ShortcutScheme* savedShortcutScheme;

public:
	Palette ();
	virtual ~Palette ();

	static Palette&	GetInstance ();

	void 	Show ();
	void 	Hide ();

	virtual void	PanelOpened (const DG::PanelOpenEvent& ev) override;
	virtual void	PanelCloseRequested (const DG::PanelCloseRequestEvent& ev, bool* accepted) override;
	virtual void	PanelResized (const DG::PanelResizeEvent& ev) override;
	virtual void	PanelHotkeyPressed (const DG::PanelHotKeyEvent& ev, bool* processed) override;

private:
	static bool		HasInstance ();
	static void		CreateInstance ();
};


}


#endif
