/*
 * Сделано в SharpDevelop.
 * Пользователь: Morigun
 * Дата: 24.11.2016
 * Время: 9:19
 */
using System;
using System.Diagnostics;
using NUnit.Framework;

namespace InventoryLib
{
	public class TestItem : IItem{
		
		public enum ElementType{EQUIP, RES, TRASH};
		int ID;
		float Cost;
		int Count;
		int MaxCount;
		bool Stacked;		
		ElementType EType;
		
		public TestItem(){
			this.ID = 0;
			this.Cost = 15;
			this.Count = 1;
			this.MaxCount = 64;
			this.Stacked = true;
			this.EType = ElementType.TRASH;
		}
		
		public TestItem(TestItem item){
			this.ID = item.ID;
			this.Cost = item.Cost;
			this.Count = item.Count;
			this.MaxCount = item.MaxCount;
			this.Stacked = item.Stacked;
			this.EType = item.EType;
		}
		
		public TestItem(TestItem item, int Count){
			this.ID = item.ID;
			this.Cost = item.Cost;
			this.Count = Count;
			this.MaxCount = item.MaxCount;
			this.Stacked = item.Stacked;
			this.EType = item.EType;
		}
		
		public TestItem(int id, float cost, int count, int maxCount, bool stacked, ElementType eType){
			this.ID = id;
			this.Cost = cost;
			this.Count = count;
			this.MaxCount = maxCount;
			this.Stacked = stacked;
			this.EType = eType;
		}
		
		public int getID()
		{
			return ID;
		}
		
		public int getCount()
		{
			return Count;
		}
		
		public void addCount(int cnt)
		{
			Count += cnt;
		}
		
		public void minusCount(int cnt)
		{
			Count -= cnt;
		}
		
		public void countToMax()
		{
			Count = MaxCount;
		}
		
		public int getMaxInStack()
		{
			return MaxCount;
		}
		
		public bool getStacked()
		{
			return Stacked;
		}
		
		public float getCost()
		{
			return Cost;
		}
		
		public IItem initItem(int count)
		{
			return new TestItem(this, count);
		}
		
		public int SortByCostAscending(int x, int y){
			return x.CompareTo(y);
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			TestItem other = obj as TestItem;
			if(other == null) return false;
			return Equals(other.ID);
		}
		
		public override int GetHashCode()
		{
			/*int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * ID.GetHashCode();
				hashCode += 1000000009 * Cost.GetHashCode();
				hashCode += 1000000021 * Count.GetHashCode();
				hashCode += 1000000033 * MaxCount.GetHashCode();
				hashCode += 1000000087 * Stacked.GetHashCode();
			}
			return hashCode;*/
			return this.ID;
		}
		
		public static bool operator ==(TestItem lhs, TestItem rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(TestItem lhs, TestItem rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
		
		public override string ToString()
		{
			return string.Format("[TestItem ID={0}, Count={1}, MaxCount={2}, Stacked={3}, Cost={4}]", ID, Count, MaxCount, Stacked, Cost);
		}
		
		public int getValForSort()
		{
			switch(this.EType){
				case ElementType.EQUIP:
					return 0;
				case ElementType.RES:
					return 1;
				case ElementType.TRASH:
					return 2;
				default:
					return 3;
			}
		}
		
		public bool Equals(IItem other)
		{
			if(other == null) return false;
			return (this.ID.Equals(other.getID()));
		}
		
		public int CompareTo(IItem other)
		{
			if(other == null)
				return 1;
			else
				return this.ID.CompareTo(other.getID());
		}
	}
	[TestFixture]
	public class TestClass
	{
		[Test]
		public void TestBuySell()
		{
			InventoryClass ic = new InventoryClass(16);
			TestItem ti = new TestItem();
			for(int i = 0; i < 16; i++){				
				ic.Add(new TestItem(ti,1));
			}			
			
			
			ic.SellItem(new TestItem(ti,2));
			ic.BuyItem(new TestItem(ti));
			NUnit.Framework.Assert.AreEqual(ic.getMoney() ,15);
		}
		
		[Test]
		public void TestAddDel(){
			InventoryClass ic = new InventoryClass(16);
			TestItem ti = new TestItem();
			ic.Add(new TestItem(ti,2));
			ic.Del(ti);
			NUnit.Framework.Assert.AreEqual(ic.getElement(0).getCount(), 1);
		}
		
		[Test]
		public void TestStacked(){
			InventoryClass ic = new InventoryClass(16);
			TestItem ti = new TestItem(0,11,1,2,true, TestItem.ElementType.TRASH);			
			ic.Add(ti);
			ic.Add(ti);
			NUnit.Framework.Assert.AreEqual(ic.getCount(), 1);
		}
		[Test]
		public void TestSort(){
			InventoryClass ic = new InventoryClass(16);
			TestItem ti = new TestItem();
			ic.Add(new TestItem(ti));
			ti = new TestItem(1,10,1,2,true,TestItem.ElementType.EQUIP);
			ic.Add(ti);
			NUnit.Framework.Assert.AreEqual(ic.getElement(0).getID(),0);
			NUnit.Framework.Assert.AreEqual(ic.getElement(1).getID(),1);
			ic.Sort(SortType.COST_SORT);
			NUnit.Framework.Assert.AreEqual(ic.getElement(1).getCost(), 10);
		}
	}
}
