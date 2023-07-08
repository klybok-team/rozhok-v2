const { createCanvas, loadImage, registerFont } = require('canvas');

registerFont('../assets/Lobster.ttf', { family: 'Lobster' });

module.exports = {
	name: 'jack_fresco',
	aliases: ['jack'],
    desc: 'Создать смешную картинку с Жаком Фреско',
	async execute(client, m, args) {

        if(args.length > 99) return m.channel.send(`Слишком много текста.\n${args.length} из 99`);

        const canvas_img = await loadImage(require('fs').readFileSync('../assets/jack_fresco.png'));

        const canvas = createCanvas(canvas_img.width, canvas_img.height);
        const ctx = canvas.getContext('2d');
    
        ctx.drawImage(canvas_img, 0, 0);

        ctx.fillStyle = '#000000';
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.font = '32px Lobster';
    
        ctx.fillText(args, 595, 200);
    
        const result = canvas.toBuffer()

        m.channel.send({ files: [{ attachment: result, name: 'jack_fresko.png' }]});
	}
};