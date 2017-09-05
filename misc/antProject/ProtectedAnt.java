class ProtectedAnt{
	private Ant ant;

	public ProtectedAnt(Ant ant){
		this.ant = ant;
	}

	public void setAnt(Ant ant){this.ant = ant;}
	public int getStamina(){return ant.getStamina();}
	public int getMaxStamina(){return ant.getMaxStamina();}
	public boolean hasFood(){return ant.hasFood();}
	public boolean checkFood(int dir){return ant.checkFood(dir);}
	public void gatherFood(){ant.gatherFood();}
	public double getFeromones(int dir){return ant.getFeromones(dir);}
	public void setFeromoneDosis(double feromoneDosis){ant.setFeromoneDosis(feromoneDosis);}
	public int getPreviousDir(){return ant.getPreviousDir();}
	public int getDynamicMemory(){return ant.getDynamicMemory();}
	public int getStaticMemory(int index){return ant.getStaticMemory(index);}
	public void setDynamicMemory(int value){ant.setDynamicMemory(value);}
	public void clearMemory(){ant.clearMemory();}
}