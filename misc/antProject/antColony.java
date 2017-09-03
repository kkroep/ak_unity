//import java.awt.BasicStroke;
import java.awt.Color;
//import java.awt.Font;
//import java.awt.FontMetrics;
//import java.awt.GradientPaint;
import java.awt.Graphics2D;
//import java.awt.geom.Ellipse2D;
import java.awt.image.BufferedImage;
import java.awt.geom.Rectangle2D;
import javax.imageio.stream.*;
import java.io.File;
import java.io.IOException;
//import javax.imageio.ImageIO;

public class antColony {
  static public void main(String args[]) throws Exception {
     try {
      int width = 64, height = 64, multiplier = 8;
      int[][][] picture = new int[width][height][3];

      // TYPE_INT_ARGB specifies the image format: 8-bit RGBA packed
      // into integer pixels
      BufferedImage bi = new BufferedImage(width*multiplier, height*multiplier, BufferedImage.TYPE_INT_ARGB);
      ImageOutputStream output = new FileImageOutputStream(new File("test.gif"));
      Graphics2D ig2 = bi.createGraphics();
      GifSequenceWriter writer = new GifSequenceWriter(output, bi.getType(), 1, false);
      //writer.writeToSequence(bi);

      // initializing main Loop
      Referee referee = new Referee(40, width, height);


      Player player1 = new Player("KeKroepes", 0, 20, 20);
      //player1.draw(picture);
      writer.writeToSequence(bi);

      // main loop
      for(int frame = 0; frame<1000; frame++){
        // clear image
        ig2.setPaint(Color.white);
        ig2.fill(new Rectangle2D.Double(0, 0, width*multiplier, height*multiplier));
        
        // execute player turns

        player1.turn(referee);
        referee.turn();


        //draw everything
        for(int i=0; i<width; i++)
          for(int j=0; j<height; j++)
            for(int k=0; k<3; k++)
              picture[i][j][k] = 25;

        referee.draw(picture);
        player1.draw(picture);  

        for(int i=0; i<width; i++)
          for(int j=0; j<height; j++)
            {
              if(picture[i][j][0]<255 || picture[i][j][1]<255 || picture[i][j][2]<255){
                ig2.setPaint(new Color(picture[i][j][0],picture[i][j][1],picture[i][j][2]));
                ig2.fill(new Rectangle2D.Double(i*multiplier, j*multiplier, multiplier, multiplier));
              }
            }


        //store the image into the gif
        writer.writeToSequence(bi);
      }

      // close the gif output
      writer.close();
      output.close();
    } catch (IOException ie) {
      ie.printStackTrace();
    }
}
}