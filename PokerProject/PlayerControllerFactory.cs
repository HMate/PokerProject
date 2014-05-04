﻿using PokerProject.PokerGame.PlayerClasses.PlayerAIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PokerProject
{
    static class PlayerControllerFactory
    {


        public static List<PlayerController> GetItemsSource()
        {
            return MakeNewList();
        }

        private static List<PlayerController> MakeNewList()
        {
            List<PlayerController> list = new List<PlayerController>();

            list.Add(new HumanController());

            return list;
        }

    }
}
