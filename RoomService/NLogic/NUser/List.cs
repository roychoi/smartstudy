using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace RoomService.NLogic.NUser
{
    public class List: IEnumerable
    {
        private Dictionary<String, User> _userList = new Dictionary<string,User>();

        public int GetCount()
        {
            return _userList.Count;
        }

        public void RemoveAll()
        {
            _userList.Clear();
        }

        public bool InsertUser(User user)
        {
            try
            {
                _userList.Add(user.UserGuid, user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public User FindUser( String user_no )
        {
            User findUser = null;
            bool result = _userList.TryGetValue(user_no, out findUser);
            if (result == true)
            {
                return findUser;
            }

            return null;
        }

        public bool RemoveUser(User user)
        {
            try
            {
                _userList.Remove(user.UserGuid);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _userList.GetEnumerator();
        }

    }
}
