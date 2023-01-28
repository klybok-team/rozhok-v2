const config = require('../../config.js')
const fs = require('fs')
module.exports = {
	name: 'dataDir',
	aliases: ['data'],
    desc: '[DEV-команда] предназначена для управления категорией /data',
    devAccess: true,
	async execute(client, m, args) {
		let guildID = m.guildID;
		if(!guildID) return client.createMessage(m.channel.id, 'Эту команду нельзя использовать в ЛС.')
		if(!args) {
            let DMChannel = await client.getDMChannel(m.author.id)
            client.createMessage(DMChannel.id, `Привет ${m.member.nick}!\n\nСписок действий:\n\`Удалить: {regex} | -yes или -y\`\n\nВся информация о regex-е доступна на этом сайте:\nhttps://regex101.com/`);
            return client.createMessage(m.channel.id, 'Отправил инфу тебе в ЛС.');
        };

		args = args.toString().split(' | ');

		let txtFile;

		if(config.ourFile === false) txtFile = fs.readFileSync(`../data/${guildID}_data.txt`, 'UTF-8').toString();
		if(config.ourFile === true) txtFile = fs.readFileSync(`../data/data.txt`, 'UTF-8').toString();

		const pattern = new RegExp(args[0], 'g')

        if(args[0] && !args[1]) {
			let DMChannel = await client.getDMChannel(m.author.id)
            client.createMessage(DMChannel.id, `Найдено ${txtFile.match(pattern)?.length || 0} совпадений (ия).\n\nДля удаления добавьте \` | -yes\` или \` | -y\` в конец\n\nТвои результаты:`, [{ file: `${txtFile.match(pattern)?.join('\n')}`, name: 'preview.txt' }]);
            return client.createMessage(m.channel.id, 'Отправил инфу тебе в ЛС.');
        }
        if(args[1] === '-yes' || args[1] === '-y') {
			
			if(config.ourFile === false) txtFile = fs.writeFileSync(`../data/${guildID}_data.txt`, txtFile.replace(pattern, ''));
			if(config.ourFile === true) txtFile = fs.writeFileSync(`../data/data.txt`, txtFile.replace(pattern, ''));

			if(config.ourFile === false) txtFile = fs.readFileSync(`../data/${guildID}_data.txt`, 'UTF-8');
			if(config.ourFile === true) txtFile = fs.readFileSync(`../data/data.txt`, 'UTF-8');
			let myHeart = []
			
			for(let string of txtFile.split('\n')) {
				if(string.length != 0) myHeart.push(string)
			}
			if(config.ourFile === false) txtFile = fs.writeFileSync(`../data/${guildID}_data.txt`, myHeart.join('\n'));
			if(config.ourFile === true) txtFile = fs.writeFileSync(`../data/data.txt`, myHeart.join('\n'));

			client.createMessage(m.channel.id, 'Если вы видите это сообщение, значит бот не крашнулся и скорее всего все прошло успешно.')
		}
	}
}