using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonQuest
{
	public class DQCharacterData
	{
		private static DQCharacterData _current;
		
		public static DQCharacterData current
		{
			get
			{
				if (_current == null)
					_current = new DQCharacterData();
				return _current;
			}
		}

		// retrieves save fild and sets as _current
		public void Setup(DQCharacterData data)
		{
			_current = data;
		}

		public enum ActionGrowthType
		{
			Flat, 
			Percent
		}
		
		public enum RedVelvet
		{
			Irene = 0, 
			Seulgi = 1, 
			Wendy = 2, 
			Joy = 3, 
			Yeri = 4
		}

		public enum Element
		{
			Pink = 0, 
			Yellow =1,
			Blue = 2,
			Green =3, 
			Purple =4
		}

		/// <summary>
		/// Character defines the special move the character has one the tap
		/// Tap happens at a specific time 
		/// </summary>
		public class Character
		{
			public RedVelvet Name;
			public int AttackDamage;
			public float AttackSpeed;
			public float CritPercent;
			public int WeaponIndex;
			public int OutfitIndex;
			public float Cooldown;
		}

		// This call has a null check so it could be expensive without a reference 
		public Dictionary<RedVelvet, Character> Characters => _characters;
		
		private static Dictionary<RedVelvet, Character> _characters = new Dictionary<RedVelvet, Character>()
		{
			{RedVelvet.Irene, CreateCharacter(RedVelvet.Irene)},
			{RedVelvet.Seulgi, CreateCharacter(RedVelvet.Seulgi)},
			{RedVelvet.Wendy, CreateCharacter(RedVelvet.Wendy)},
			{RedVelvet.Joy, CreateCharacter(RedVelvet.Joy)},
			{RedVelvet.Yeri, CreateCharacter(RedVelvet.Yeri)},
						
		};

		private static Character CreateCharacter(RedVelvet name)
		{
			var character = new Character();
			character.Name = name;
			character.CritPercent = 0.01f;
			character.AttackDamage = 1;
			character.OutfitIndex = 0;
			return character;
		}

		/// <summary>
		/// All cards can level up to 99 -> all leveling is the same for all cards
		/// Cards can evolve 3-5 times -> changes the action growth
		/// can have an effect -> takes parameter Card and Character
		/// rarity effects -> takes parameter Card -> can effect base stats or price inflation or action growth
		/// </summary>
		public class Card
		{
			public Character Character;
			public Sprite[] Sprites;
			public Element Element;
			public int CardNumber;
			public float Action;
			public float ActionGrowth;
			public ActionGrowthType Type;
			public int Rarity;
			public int StartPrice;
			public float PriceInflation;
			public Action<Character, Card> Effect;
			public Action<Card> RarityEffect;
		}

		private static Dictionary<RedVelvet, List<Card>> _cardCollection = new Dictionary<RedVelvet, List<Card>>();
		public static Dictionary<RedVelvet, List<Card>> CardCollection => _cardCollection;

	}

}

