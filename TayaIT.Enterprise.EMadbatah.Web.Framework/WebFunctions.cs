using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Web.Framework
{
    public class WebFunctions
    {
        public enum AdminFunctions
        {
            GetAllUsersFromDB, //IN= None, OUT=HTML
            GetAllUsersFromAD, //IN= None, OUT=HTML
            GetRevUsersFromDB,
            ActivateUser, //IN= userId, OUT=bool
            DeActivateUser, //IN= userId, OUT=bool
            RemoveUser, //IN= userId, OUT=bool
            UpdateUser, //IN= userId, OUT=bool
            UnlockSessionFile, //IN= sessionFileId, OUT=bool
            UnlockSessionFileReviewer,
            AssignSessionFile, //IN= sessionFileID, userID, OUT=bool
            AssignSessionFileReviewer,
            UpdateServersPaths, //IN= string, string, OUT=bool
            AddNewUser,
            ChangeSessionReviewer,
            UnlockSessionReviewer,            
            SignOut
        }

        public enum SessionsFunctions
        {
            GetAllSessions, //IN= string, OUT=bool
            CreateNewSession, //IN= string, OUT=bool
            AssignSessionStart, //IN userID, sessionStartID, OUT=bool
            GetSessionFiles, //IN sessionID, OUT=HTML
            ReorderSessionFiles,//IN sessionFileID, sessionFileOldOrder, sessionFileNewOrder, OUT=bool
            AddAttachment,
            RemoveAttachment,
            ReOrderAttachment,//IN attachementID, attachementOldOrder, attachementNewOrder, OUT=bool
            GetReviewNotesSummaryForDataEntry, // IN= sessionID, out=HTML
            GetReviewNotesSummaryForReviewer, // IN= sessionID, out=HTML
            ConfirmNewSession,
            CreateMadbatahFiles,
            ReloadSessionFiles,
            GetSpeakersStatistics,
            GetSessionAgendaItemsIndex,
            UpdateSessionAgendaItemValue,
            UpdateSessionInfo,
            UpdateAttendantSessionTitle
        }

        public enum ReviewFunctions
        {
            GetMadbatahAsHTML, //IN= sessionId, OUT=HTML
            RejectSessionContentItem,
            ApproveSessionContentItem,
            UpdateSessionContentItemText,
            ApproveSession,
            FinalApproveSession,
            GetUserReviewNotes, //in SessionID, int userID
            GetSessionStatus, //in SessionID, int userID
            IsSessionFileLockedByFileRev,
            ApproveRejectedItemsInFile
        }
        public enum SessionStartFunctions
        {
            ReloadAutomaticSessionStart,
            SaveSessionStart,
            RejectSessionStart,
            ApproveSessionStart
        }

        public enum EditWizard
        {
            GetAgendaSubItems,
            AddAgendaItem,
            UpdateAgendaItem,
            DoNext,
            DoPrevious,
            BackToOriginalContent,
            SaveAndExit,
            StartFromBeginning,
            ContinueEdit,
            GetAllAgendaItems,
            CreateCustomAgendaItem,
            GetSpeakersList,
            SaveSessionContentItem,
            UpdateSessionFileStatusCompleted,
            SplitItem,
            GetSpeakerJobTitle
        }

        public enum FileFunctions
        {
            Upload,
            Download,
            GetFilesStatus
        }
    }
}
