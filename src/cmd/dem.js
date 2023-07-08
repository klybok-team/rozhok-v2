const downloadFile = require('../functions/downloadFile.js')
const demotivatorImage = require('../functions/demotivatorImage.js');

module.exports = {
	name: 'demotivator',
	aliases: ['d', 'dem'],
	desc: 'Создать смешной демотиватор',
	async execute(client, m, args) {

		args = args.toString().split(' | ');

		if(!args[0]) return client.createMessage(m.channel.id, 'Не указан текст.');

		let attachment;

		if(!m.attachments[0]) {
			if(m.messageReference?.messageID && m.messageReference?.channelID) {
				let messageReferenceAttachments = await client.getMessage(m.messageReference.channelID, m.messageReference.messageID)

				attachment = messageReferenceAttachments?.attachments[0];
			};
		};

		if(m.attachments[0]) attachment = m.attachments[0];
	
		if(!attachment) return m.channel.send('Отсутствует изображение.');

		if(attachment.filename.toLowerCase().endsWith('.jpg') || attachment.filename.toLowerCase().endsWith('.jpeg') || attachment.filename.toLowerCase().endsWith('.png')) {
			if(args[0].length > 35 || args[1]?.length > 35) return m.channel.send(`Упс.. много букв.\nТекст: ${args[0].length} из 35\nПод-текст ${args[1]?.length ? args[1].length : '0'} из 35`);
			return await downloadFile(`${attachment.url}`).then(async source => {
			let image = await demotivatorImage(source, args[0], args[1]);

			m.channel.send({ files: [{ attachment: image, name: attachment.filename }]});
		});
	};
		return m.channel.send('Расширение файла не поддерживается.');
	}
};