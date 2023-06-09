using System;
using Autodesk.AdvanceSteel.CADAccess;
#if ADVANCESTEEL2023
using Autodesk.AdvanceSteel.DocumentManagement;
#else
using Autodesk.AutoCAD.DatabaseServices;
#endif

namespace Speckle.ConnectorAutocadCivil.DocumentUtils;

#if ADVANCESTEEL2023
  public class TransactionContext : IDisposable
  {
    private bool DocumentLocked;
    private Transaction Transaction;

    private TransactionContext(Document document)
    {
      if (!DocumentLocked) DocumentLocked = DocumentManager.LockCurrentDocument();

      if (Transaction == null && DocumentLocked) Transaction = TransactionManager.StartTransaction();
    }

    public void Dispose()
    {
      Transaction?.Commit();
      Transaction = null;

      if (DocumentLocked)
      {
        DocumentManager.UnlockCurrentDocument();
        DocumentLocked = false;
      }
    }

    public static TransactionContext StartTransaction(Document document)
    {
      return new TransactionContext(document);
    }
  }
#else
public class TransactionContext : IDisposable
{
  private DocumentLock DocumentLock;
  private Transaction Transaction;

  private TransactionContext(Document document)
  {
    DocumentLock = document.LockDocument();
    Transaction = document.Database.TransactionManager.StartTransaction();
  }

  public void Dispose()
  {
    Transaction?.Commit();
    Transaction = null;

    DocumentLock?.Dispose();
    DocumentLock = null;
  }

  public static TransactionContext StartTransaction(Document document)
  {
    return new TransactionContext(document);
  }
}
#endif
