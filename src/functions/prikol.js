const { createCanvas, loadImage, registerFont } = require('canvas');
const random = require('../functions/random.js');

registerFont('../assets/Lobster.ttf', { family: 'Lobster' });

module.exports = async(text, whatYouNeed, img) => {
    if(whatYouNeed === 'textOnImg') {

    const canvas_img = await loadImage(img);

    const canvas = createCanvas(canvas_img.width, canvas_img.height);
    const ctx = canvas.getContext('2d');

    ctx.drawImage(canvas_img, 0, 0);

    ctx.fillStyle = '#fff'
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.font = '32px Lobster';


    ctx.fillText(text, random(0, canvas_img.width), random(0,  canvas_img.height));

    return canvas.toBuffer();
    };
    if(whatYouNeed === 'jack_fresco') {
        const canvas_img = await loadImage(require('fs').readFileSync('../assets/jack_fresco.png'));

        const canvas = createCanvas(canvas_img.width, canvas_img.height);
        const ctx = canvas.getContext('2d');
    
        ctx.drawImage(canvas_img, 0, 0);

        ctx.fillStyle = '#000000';
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.font = '32px Lobster';
    
    
        ctx.fillText(text, 595, 200);
    
        return canvas.toBuffer();
    };
};
