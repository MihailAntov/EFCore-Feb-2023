﻿using P02_FootballBetting.Data;

namespace P02_FootballBetting;

public class StartUp
{
    static void Main(string[] args)
    {
        using (FootballBettingContext context = new FootballBettingContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


        }
    }
}
