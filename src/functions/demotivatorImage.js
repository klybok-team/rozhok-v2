const { createCanvas, loadImage } = require('canvas');

module.exports = async(img, title, subtitle) => {
  const canvas = createCanvas(512, 512);
  const ctx = canvas.getContext('2d');
    
  const asset = await loadImage('../assets/demotivator.png');
  ctx.drawImage(asset, 0, 0);
  
  const avatar = await loadImage(img);
  ctx.drawImage(avatar, 33, 33, 446, 390);
    
  ctx.fillStyle = '#fff';
  ctx.textAlign = "center";
  ctx.textBaseline = "middle";
  ctx.font = `28px Times New Roman`;
  
  let widthText = 470;
  if(subtitle) ctx.fillText(title, 260, widthText - 10)
  if(!subtitle) ctx.fillText(title, 260, widthText);
  
  if(subtitle) {
  ctx.font = `20px Times New Roman`;
  ctx.fillText(subtitle, 260, widthText + 20);
  };
  return canvas.toBuffer();
};