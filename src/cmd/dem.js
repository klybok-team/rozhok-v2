const { demotivatorReplies, other } = require('../../config.js')
const downloadFile = require('../functions/downloadFile.js')
const demotivatorImage = require('../functions/demotivatorImage.js');

module.exports = {
	name: 'demotivator',
	aliases: ['d', 'dem'],
	async execute(client, m, args) {
		args = args.toString().split(' | ');

		if(!args[0]) return client.createMessage(m.channel.id, demotivatorReplies.ErrorArgs.noText);

		let attachment;

		if(!m.attachments[0]) {
			if(m.messageReference?.messageID && m.messageReference?.channelID) {
				let messageReferenceAttachments = await client.getMessage(m.messageReference.channelID, m.messageReference.messageID)

				attachment = messageReferenceAttachments?.attachments[0];
			};
		};
		if(m.attachments[0]) attachment = m.attachments[0];
	
		if(!attachment) return client.createMessage(m.channel.id, demotivatorReplies.ErrorArgs.noImg);

		if(attachment.filename.endsWith('.jpg') || attachment.filename.endsWith('.jpeg') || attachment.filename.endsWith('.png')) {

		if(args[0].length > 35 || args[1]?.length > 35) return client.createMessage(m.channel.id, demotivatorReplies.Errorlength.replace('{}', args[0].length).replace('{1}', args[1]?.length ? args[1].length : '0'));
		return await downloadFile(`${attachment.url}`).then(async source => {

		let image = await demotivatorImage(source, args[0], args[1]);
		client.createMessage(m.channel.id, {}, [{ file: image, name: attachment.filename }]);
		});
		};
		return client.createMessage(m.channel.id, other.fileExNotNed);
	},
};