const fs = require('fs');

if (fs.existsSync('../config_example.js') && !fs.existsSync('../config.js')) {
    console.log('config_example.js был переименован в config.js');
    fs.renameSync('../config_example.js', '../config.js');
};

let config = require('../config.js');

if (!config.token) return console.log('Отсутствует токен клиента для его запуска..');

const { Client, Collection, GatewayIntentBits, PresenceUpdateStatus, ActivityType, DMChannel } = require('discord.js');

const client = new Client({
    /*
    // Readd this, idk how to fix
    allowedMentions: {
        roles: false,
        users: false,
        repliedUser: true,
    },*/
    intents: [GatewayIntentBits.Guilds, GatewayIntentBits.GuildMessages, GatewayIntentBits.MessageContent, GatewayIntentBits.GuildPresences]
});

client.cmd = new Collection();

const random = require('./functions/random.js');
const write = require('./functions/write.js');
const downloadFile = require('./functions/downloadFile.js');

for (const nameOfFile of fs.readdirSync('./cmd').filter(file => file.endsWith('.js'))) {
    const command = require(`./cmd/${nameOfFile}`);
    client.cmd.set(command.name, command);
};

client.on("messageCreate", async (m) =>
{
    if (m.author.bot) return;

    if (m.content.startsWith(config.CommandsPrefix) && config.CommandsEnable === true)
    {

        const args = m.content.slice(config.CommandsPrefix.length).trim().split(/ +/);
        const nameOfCommand = args.shift();

        const command = client.cmd.get(nameOfCommand) || client.cmd.find(cmd => cmd.aliases && cmd.aliases.includes(nameOfCommand));

        if (!command) return;

        let areYouDev = false;
        for (let userID of config.CommandsAccess) if (userID === m.author.id) areYouDev = true;

        try {
            if (command.devAccess && areYouDev === false) return m.channel.send('У вас нет прав на использование этой команды.')
            await command.execute(client, m, args.join(' '), areYouDev);
        } catch (e) {
            console.log(e);
            m.channel.send(config.erorr);
        };
        return;
    };

    if (m.channel == DMChannel || m.guild.id == null) return;

    let data;

    if (config.OurFile === false)
    {
        if (!fs.existsSync(`../data/${m.guild.id}_data.txt`))
        {
            console.log('Обнаружена новая гильдия, создаю файл с текстом для неё..');

            fs.writeFileSync(`../data/${m.guild.id}_data.txt`, 'привет');
        };

        data = fs.readFileSync(`../data/${m.guild.id}_data.txt`, 'UTF-8');
    }

    if (config.OurFile === true)
    {
        data = fs.readFileSync(`../data/data.txt`, 'UTF-8');
    }

    let lines = data.split(/\r?\n/);
    let imgdir = fs.readdirSync('../img');

    let trueOrNot = false;
    for (let idChannel of config.idChanneltoSaveAndWrite) {
        if (m.channel.id === idChannel) trueOrNot = true;
    };
    if (!trueOrNot) return;

    if (m.content.length < config.MaxLenghtToWrite && config.SaveAnyData)
    {
        let contentmessage = m.content.split('\n').join(' ');

        if (m.content.match("(https?://)?(www.)?(discord.(gg|io|me|li)|discordapp.com/invite)/[^\s/]+?(?=\s)"))
        {
            console.log("Обнаружено сообщение с дискорд-ссылкой..")

            if (config.MsgFilter == "links") return;
        }

        if (m.content != null && m.content != `<@${client.user.id}>`) write(contentmessage, config.OurFile, m.guild.id);

        if (config.ImgSaveAndUse && imgdir.length < config.Limitimg) {
            for (let attachment of m.attachments) {
                if (attachment.filename.endsWith('.jpg') || attachment.filename.endsWith('.jpeg') || attachment.filename.endsWith('.png')) {
                    let saveThisBoolean = false;
                    if (config.imageFilter === 'none') saveThisBoolean = true;
                    if (saveThisBoolean === true && !m.attachments[config.LimitToImgOnce]) {
                        await downloadFile(`${attachment.url}`).then(async source => {
                            fs.createWriteStream(`../img/${m.id}_${attachment.filename.replace('.jpg', '.jpeg')}`).write(source);
                        });
                        console.log(`Скачан файл: ${attachment.filename}`);
                    };
                }
            }
        };
    };

    if (!m.mentions.has(client.user)) {
        if (!config.RandomMessage) return;
        if(random(0, 11) < config.MessageChance) return;
    }

    if (config.ImgSaveAndUse) {
        if (random(0, 11) < 4) {
            const file = imgdir[random(0, imgdir.length)];
            try {
                let img = fs.readFileSync(`../img/${file}`);
                if (random(0, 5) === 2) return m.channel.send({ files: [{ attachment: img, name: file }]});
                return m.channel.send(lines[random(0, lines.length)], { files: [{ attachment: img, name: file }]})
            } catch (e) {
                console.error(e);
            };
        };
    };
    if (random(0, 3) === 1) {
        return m.channel.send(lines[random(0, lines.length)] + ' ' + lines[random(0, lines.length)]);
    };
    return m.channel.send(lines[random(0, lines.length)]);
});

client.once("ready", () => {
    if (config.CommandsAccess[0].length == 0) console.warn('Не назначены те, кто имеют доступ к DEV-командам');

    if (fs.readdirSync('../img').length >= config.limitimg) {
        console.log(`\x1b[31mМесто в ../img закончилось. ${fs.readdirSync('../img').length} из ${config.limitimg}\x1b[0m`);
    };

    console.log(`Готов,\nИмя: ${client.user.username}\nДОП. ИНФ.\nСсылка на аватарку: ${client.user.displayAvatarURL()}\nАйди клиента: ${client.user.id}\nКоличество серверов: ${client.guilds.cache.size}\nРандомное число для откладки: ${random(0, 100)}`);
    if (!config.ClientOnlineStatus)
    {
        console.log('У вас не указан онлайн-статус бота');
    };
    if (!config.ClientTextStatus || !config.TypeOfStatus)
    {
        console.log('У вас не указан тип статуса или текст статуса, отключаем..');
        return;
    };
    if (config.TypeOfStatus == ActivityType.Streaming)
    {
        client.user.setActivity(config.ClientTextStatus, { type: config.TypeOfStatus, url: config.TwitchURL, name: config.ClientTextStatus })
        return;
    };
    client.user.setActivity(config.ClientTextStatus, { type: config.TypeOfStatus, url: config.TwitchURL, name: config.ClientTextStatus })
    return client.user.setStatus(config.ClientOnlineStatus);
});

client.on('error', (e) => {
    console.error(e);
});

client.login(config.token);