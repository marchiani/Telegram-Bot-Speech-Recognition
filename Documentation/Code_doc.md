### Документація по коду

##### 1. Підключення бота і його функціоналу

       private AzureSpeechToTextSettign AzureSpeechToText =>    Configuration.GetSection("AzureSpeechToText").Get<AzureSpeechToTextSettign>();
              private TelegramBotSettings TelegramBotSettings => Configuration.GetSection("TelegramBotSettings").Get<TelegramBotSettings>();
              private ZamzarSettings ZamzarSettings => Configuration.GetSection("ZamzarSettings").Get<ZamzarSettings>();
              private SmallTalkSettings SmallTalkSettings => Configuration.GetSection("SmallTalkSettings").Get<SmallTalkSettings>();

              public Startup(IWebHostEnvironment env)
              {
                  var builder = new ConfigurationBuilder()
                      .SetBasePath(env.ContentRootPath)
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();

                  Configuration = builder.Build();
              }

##### 2. За допомогою бібліотеки ApiAiSDK передається Token для зв'язку з агентом Dialogflow
        public class SmallTalkSettings
            {
                public string Token { get; set; }
            }
        "SmallTalkSettings": {"Token": "Bot's Token"}


##### 3. Відправлення ботом повідомлень до клієнта
        public async void SpeakWithBot(string textMessage, long chatId)
                {
                    var responseFromApi = ApiAi.TextRequest(textMessage);
                    string result = responseFromApi.Result.Fulfillment.Speech;

                    if (result == "")
                    {
                        result = "Простите, я не понял, повторите еще раз, пожалуйста.";
                    }

                    await TelegramBotClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: result
                    );
                }

За допомогою метода **SendTextMessageAsync** повертається відповідь на повідомлення, якщо його не існує, повертає "Простите, я не понял, повторите еще раз, пожалуйста."

##### 4. Створення додаткових кнопок

        var aboutKeyboard = new ReplyKeyboardMarkup(new[]
                            {
                                new[]
                                {
                                    new KeyboardButton("Кто ты?")
                                },
                                new[]
                                {
                                    new KeyboardButton("Что ты можешь?")
                                },
                                new[]
                                {
                                    new KeyboardButton("Расскажи про своё хобби")
                                }
                            });
За допомогою метода **KeyboardButton** створюються нові кнопки

##### 5. Привласнення команд боту
        public async Task TextMessages(Message message)
                {
                    switch (message?.Text.Split(' ').First().Trim('/', ' ').ToLower())
                    {
                        case "menu":
                            string menuText =
        @"Menu of TaraBot:
        /keyboard - действия
                            await TelegramBotClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: menuText
                            );

                            break;
                        case "keyboard":
                            var infoKeyboard = new ReplyKeyboardMarkup(new[]
                            {
                                new[]
                                {
                                    new KeyboardButton("Связаться с нами!") {RequestContact = true}
                                }
                            });

##### 6. Визначення типу повідомлення (голосове, аудіофайл або текстове)

        public async Task DefineCommand()
                {
                    var message = Update.Message;

                    switch (message.Type)
                    {
                        case MessageType.Voice:
                            await VoiceTranscriberService.SpeechToText(message);
                            break;
                        case MessageType.Audio:
                            await VoiceTranscriberService.SpeechToText(message);
                            break;
                        case MessageType.Text:
                            await TextMessages(message);
                            break;
                        default:
                            break;
                    }
                }                        
##### 7. Конвертація аудіофайлу в формат, який підтримується ботом
      var zamzarJob = ZamzarHelper.Upload<ZamzarJobResponseModel>(ZamzarSettings.SecretKey, jobEndpoint, pathToAudio, "wav").Result;

                  string getStatusConvertingEndpoint = $"{jobEndpoint}/{zamzarJob.Id}";

                  while (true)
                  {
                      zamzarJob = ZamzarHelper.GetSimpleResponse<ZamzarJobResponseModel>(ZamzarSettings.SecretKey, getStatusConvertingEndpoint).Result;
                      if (zamzarJob.Status == "successful")
                      {
                          break;
                      }
                  }

                  string downloadConvertedFileEndpoint = $"{ZamzarSettings.BaseAPIUrl}files/{zamzarJob.TargetFiles.First().Id}/content";
                  pathToAudio = Path.ChangeExtension(pathToAudio, ".wav");

                  await ZamzarHelper.Download(ZamzarSettings.SecretKey, downloadConvertedFileEndpoint, pathToAudio);

##### 8. Транскрибація (визначення подальших дій для відповіді на голосове повідомлення або аудіофайл)
      using var audioInput = AudioConfig.FromWavFileInput(pathToAudio);
                  using var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig, audioInput);

                  var speechRecognitionResult = await recognizer.RecognizeOnceAsync();
