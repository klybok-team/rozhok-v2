const downloadFile = require('../functions/downloadFile.js')
const { createCanvas, loadImage, registerFont } = require('canvas');
const random = require('../functions/random.js')

registerFont('../assets/Lobster.ttf', { family: 'Lobster' });

module.exports = {
	name: 'lobster',
	aliases: ['l'],
	desc: 'Создать смешную надпись на картинке',
	async execute(client, m, args) {
		if(!args) return client.createMessage(m.channel.id, 'Не указан текст.');

        if(args.length > 99) return client.createMessage(m.channel.id, `Слишком много текста.\n${args.lenght} из 99`);

		let attachment;

		if(!m.attachments[0]) {
			if(m.messageReference?.messageID && m.messageReference?.channelID) {
				let messageReferenceAttachments = await client.getMessage(m.messageReference.channelID, m.messageReference.messageID)

				attachment = messageReferenceAttachments?.attachments[0];
			};
		};
		if(m.attachments[0]) attachment = m.attachments[0];
	
		if(!attachment) return client.createMessage(m.channel.id, 'Отсутствует изображение.');
		
		if(attachment.filename.toLowerCase().endsWith('.jpg') || attachment.filename.toLowerCase().endsWith('.jpeg') || attachment.filename.toLowerCase().endsWith('.png')) {
            return await downloadFile(`${attachment.url}`).then(async source => {

			const canvas_img = await loadImage(source);
			const canvas = createCanvas(canvas_img.width, canvas_img.height);
			const ctx = canvas.getContext('2d');
	
			ctx.drawImage(canvas_img, 0, 0);
			
			ctx.fillStyle = '#fff'
			ctx.textAlign = "center";
			ctx.textBaseline = "middle";
			ctx.font = '32px Lobster';
			
			ctx.fillText(args, random(0, canvas_img.width), random(0, canvas_img.height));

			const result = canvas.toBuffer();

            client.createMessage(m.channel.id, {}, [{ file: result, name: attachment.filename }]);
		    });
	    } 
	return client.createMessage(m.channel.id, 'Расширение файла не поддерживается.');
	}
};