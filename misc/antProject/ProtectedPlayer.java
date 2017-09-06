import java.awt.Graphics2D;
import java.awt.Color;
import java.awt.geom.Rectangle2D;
import java.util.*;

class ProtectedPlayer{
	private Player player;

	public ProtectedPlayer(Player player){
		this.player = player;
	}

	public int colonySize(){return player.colonySize();}
	
	// returns true if it is succesfull and false if not
	public boolean createAnt(){return player.createAnt();}

	public int getFood(){return player.getFood();}

}





