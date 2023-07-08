const fs = require('fs')
module.exports = {
	name: 'imgDir',
	aliases: ['img'],
    desc: '[DEV-команда] предназначена для управления категорией /img',
    devAccess: true,
	async execute({}, m, args)
    {
        if(!args) {
            const imgList = fs.readdirSync('../img').join('\n')
            
            m.author.send(`Привет ${m.author.name}!\n\nСписок действий:\n\`Удалить\`: {имя файла} del\n\`Показать\`: {имя файла}\n\nСписок файлов:`, [{ file: `${imgList}`, name: 'preview.txt' }]);
            return m.channel.send('Отправил инфу тебе в ЛС.');
        };

        args = args.toString().split(' ');

        if(args[0] && !args[1]) {
            if(fs.existsSync(`../img/${args[0]}`)) {
                const img = fs.readFileSync(`../img/${args[0]}`);
            
                return m.channel.send('Для удаления добавьте `del -yes` или `del -y` в конец', [{ file: img, name: args[0] }])
            }
            returnm.channel.send('Такого файла нет.')
        }
        if(args[1] === 'del') {
            if(args[2] === '-yes' || args[2] === '-y') {
                if(fs.existsSync(`../img/${args[0]}`)) {
                    fs.rmSync(`../img/${args[0]}`)
                    return m.channel.send('Если вы видите это сообщение, значит бот не крашнулся и скорее всего все прошло успешно.');
                }
                return m.channel.send('Такого файла нет.')
            }
        }
	}
};