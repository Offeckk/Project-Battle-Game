﻿using ProjectDemo.Models.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDemo.Models.Fighters
{
    public abstract class Fighter
    {
        private const int FistAttackPoints = 250;

        private string name;

        private int health;

        private int maxHealth;

        private int damage;

        private int defense;

        private Dice dice;

        private string message;

        private List<Weapon> weapons;


        public Fighter(string name, Dice dice)
        {
            this.Name = name;
            this.Health = health;
            this.MaxHealth = health;
            this.Damage = FistAttackPoints;
            this.Defense = defense;
            this.Dice = dice;
            this.Weapons = new List<Weapon>();

        }

        public List<Weapon> Weapons { get => weapons; private set => weapons = value; }
        public string Name
        {
            get => name;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be null or white space!");
                }
                this.name = value;
            }
        }
        public int Health { get => health; set => health = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        public int Damage { get => damage; set => damage = value; }
        public int Defense { get => defense; set => defense = value; }
        public Dice Dice { get => dice; set => dice = value; }
        public string Message { get => message; set => message = value; }

        public override string ToString()
        {
            return name;
        }

        public bool Alive()
        {
            return (health > 0);
        }

        public string GraphicalBar(int current, int maximum)
        {
            string s = "";
            int total = 20;
            double count = Math.Round(((double)current / maximum) * total);
            if ((count == 0) && (Alive()))
            {
                count = 1;
            }
            for (int i = 0; i < count; i++)
                s += "█";
            s = s.PadRight(total);
            return s;
        }

        public string HealthBar()
        {
            return GraphicalBar(health, maxHealth);
        }

        public virtual void Attack(Fighter enemy)
        {
            //Remove broken weapons
            foreach (var weapon in weapons)
            {
                if (weapon.IsBroken)
                {
                    weapons.Remove(weapon);
                }
            }

            //Initialize weapons as damage
            int totalWeaponDamage = 0;

            //Check if there is weapon
            if (this.Weapons.Count == 0)
            {
                totalWeaponDamage = 0;
            }
            else
            {
                foreach (var weapon in weapons)
                {
                    totalWeaponDamage += weapon.AttackPoints;
                }

            }

            //Presenting the actual damage
            int hit = 0;

            //Dice add randomness
            if (totalWeaponDamage == 0)
            {
                hit = damage + dice.Roll();
            }
            else
            {
                hit = totalWeaponDamage + dice.Roll();
            }

            SetMessage(String.Format("{0} attacks with {1} hp hit", name, hit));
            enemy.Defend(hit);
        }

        public virtual void Defend(int hit)
        {
            int injury = hit - (defense + dice.Roll());
            if (injury > 0) // Injured
            {
                Health -= injury;
                message = String.Format("{0} got an injury of {1} hp", name, injury);
                if (health <= 0)
                {
                    Health = 0;
                    message += " and died";
                }
            }
            else // No damage taken
                message = String.Format("{0} parried a hit", name);
            SetMessage(message);
        }

        protected void SetMessage(string message)
        {
            this.message = message;
        }

        public string GetLastMessage()
        {
            return message;
        }

        public void TakeWeapon(Weapon weapon)
        {
            if (weapon.IsTaken)
            {
                throw new InvalidOperationException("This weapon is already taken.");
            }

            if (this.Weapons.Count() == 2)
            {
                throw new InvalidOperationException("Can't hold more weapons!");
            }

            Weapons.Add(weapon);
            weapon.IsTaken = true;
        }
    }
}
