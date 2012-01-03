using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomService.NLogic.NRoom
{
    public class Handler
    {
        public bool Enter(Room room, User enterUser)
        {
            return false;
        }
        public bool Leave(Room room, User enterUser)
        {
            return false;
        }

        public bool Commit(Room room, User user)
        {
            return false;
        }
    }
}
