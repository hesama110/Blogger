using System;
using System.Collections.Generic;
using System.Text;

namespace Blogger.Model.Enums
{
    /// <summary>
    /// Blog status is showing that the post in which state it is 
    /// </summary>
    public enum BlogStatus
    {
        Publish,  // Visible by everyone. (publish)
        Future, //Scheduled to be published in a future date. (future)
        Draft, //Post is not complete and Visible by anyone with proper user role. (draft)
        Pending, //It is Wait for a user with the role capable of publishing. (pending)
        Private, //Visible only to Administrator level. (private)
        Trash, //Posts as the Trash status. (trash)
               //["Auto-Draft"]
        AutoDraft, //It means it is save as draft but machine saved that automatically during editing. (auto-draft)
        Inherit //If the post is a child post and inheriting from a post parent. (inherit)

    }
}
