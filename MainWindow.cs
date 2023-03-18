using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.IO;
using System.Threading;
using System.Globalization;

namespace Stt_to_tts
{
    public partial class MainWindow : Form
    {
        private delegate void TextDelegate(string text);
        private SpeechSynthesizer synthesizer;
        private SpeechSynthesizer memorySynthesizer;
        private SpeechRecognitionEngine recognizer;
        private List<string> playQueue;
        private WaveOut waveout;
        private WaveIn wavein; 
        private BufferedWaveProvider wavestream;
        private Thread threadSpeakRequest;
        private Thread threadSpeakRecognition;
        private bool closed;
        private bool canceled;
        private bool completed;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            closed = false;
            canceled = true;
            playQueue = new List<string>();
            synthesizer = new SpeechSynthesizer();
            memorySynthesizer = new SpeechSynthesizer();
            wavestream = new BufferedWaveProvider(new WaveFormat(16000, 1));
            wavestream.DiscardOnBufferOverflow = true;
            for (int i = -1; i < WaveOut.DeviceCount; i++)
            {
                var temp = WaveOut.GetCapabilities(i);

                cbOutputList.Items.Add($"{i} : {temp.ProductName}");
                // populates drop down menu with audio devices
            }
            cbOutputList.Items.Add("None");
            for (int i = -1; i < WaveIn.DeviceCount; i++)
            {
                var temp = WaveIn.GetCapabilities(i);

                cbInputList.Items.Add($"{i} : {temp.ProductName}");
                // populates drop down menu with audio devices
            }
            cbInputList.Items.Add("None");
            threadSpeakRecognition = new Thread(remoteRec);
            threadSpeakRequest = new Thread(remotePlayLoop);
        }

        private void cbOutputList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ComboBox cbTemp = (ComboBox)sender;
            int selection = cbTemp.SelectedIndex - 1;
            if (selection == cbTemp.Items.Count - 2) // return if none selected
                return;
            waveout = new WaveOut();
            waveout.DeviceNumber = selection;
            Console.WriteLine(selection);
        }

        private void cbInputList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbTemp = (ComboBox)sender;
            int selection = cbTemp.SelectedIndex - 1;
            if (selection == cbTemp.Items.Count - 2) // return if none selected
                return;
            wavein = new WaveIn { WaveFormat = new WaveFormat(16000, 1) };
            wavein.DeviceNumber = selection;
            Console.WriteLine(selection);
            
        }

        private void speakAudio(string text) // makes it easy to call this from more sources
        {
            if (waveout != null)
            {
                SpeechAudioFormatInfo speechAudioFormatInfo = new SpeechAudioFormatInfo(16000, AudioBitsPerSample.Sixteen, AudioChannel.Mono);
                Stream memoryStream = new MemoryStream();
                IWaveProvider provider = new RawSourceWaveStream(memoryStream, new WaveFormat(16000, 16, 1));
                memorySynthesizer.SetOutputToAudioStream(memoryStream, speechAudioFormatInfo);
                memorySynthesizer.Speak(text);
                memorySynthesizer.SetOutputToNull();
                memoryStream.Position = 0; // reset stream position to 0
                waveout.Init(provider);
                waveout.Play();
                Console.WriteLine("Not Null");
                Console.WriteLine(memoryStream.Length);
                Thread thread = new Thread(() => waitForPlayFinish(memoryStream));
                thread.Start();

            }
            synthesizer.SpeakAsync(text);
        }



        private void btTtsPlay_Click(object sender, EventArgs e)
        {
            speakAudio(tbTtsEntry.Text);
           
        }

        private void waitForPlayFinish(Stream stream)
        {
            while(waveout.PlaybackState != PlaybackState.Stopped)
            {
                Thread.Sleep(300);
                //Console.WriteLine(waveout.PlaybackState);
            }
            //Console.WriteLine("Stopped");
            stream.Close();// be sure to close stream when done to prevent funky stuff
        }


        private void btTtsStop_Click(object sender, EventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();
            if(waveout != null)
            {
                waveout.Stop();
            }
        }


        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();
            closed = true;
        }

        private void btSttStart_Click(object sender, EventArgs e)
        {
            if(cbInputList.SelectedIndex - 1 == cbInputList.Items.Count - 2 || cbInputList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select input device", "Input Device Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (threadSpeakRecognition.IsAlive)
            {
                MessageBox.Show("Stt is already running.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            plSttThreadIndicator.BackColor = Color.Green;
            canceled = false;

            threadSpeakRequest.Start();
            threadSpeakRecognition.Start();
            
        }

        private void btSttStop_Click(object sender, EventArgs e)
        {
            canceled = true;
            plSttThreadIndicator.BackColor = Color.Maroon;
        }

        private void WriteSafe(string text) // show what text it has found in form safely
        {
            if (tbSttResults.InvokeRequired)
            {
                var d = new TextDelegate(WriteSafe);
                tbSttResults.Invoke(d, new object[] { text });
            }
            else
            {
                tbSttResults.Text = text;
            }
        }


        private void remotePlayLoop()
        {
            while (!closed && !canceled)
            {
                if (playQueue.Count > 0)
                {
                    // Console.WriteLine("Readout" + readOuts[0]);
                    speakAudio(playQueue[0]);
                    playQueue.RemoveAt(0);
                    playQueue.TrimExcess();
                }

                Thread.Sleep(100);
            }

            threadSpeakRequest = new Thread(remotePlayLoop);
        }

        private void remoteRec()
        {
            Stream buffStream = new SpeechStreamer(4096);
            wavein.StartRecording();
            wavein.DataAvailable += (s, a) =>
            {
                buffStream.Write(a.Buffer, 0, a.BytesRecorded); // constantly write to beginning of speechstream to allow for continuous live recognition
            };
            completed = false;
            // Create an in-process speech recognizer for the en-US locale.  
            using (
            recognizer = new SpeechRecognitionEngine(new CultureInfo("en-US")))
            {

                // Create and load a dictation grammar.  
                recognizer.LoadGrammar(new DictationGrammar());

                // Add a handler for the speech recognized event.  
                recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognizedHandler);
                recognizer.RecognizeCompleted += (o, e) =>
                {
                   /* if (e.Error != null)
                    {
                        Console.WriteLine("Error occurred during recognition: {0}", e.Error);
                    }
                    else if (e.InitialSilenceTimeout)
                    {
                        Console.WriteLine("Detected silence");
                    }
                    else if (e.BabbleTimeout)
                    {
                        Console.WriteLine("Detected babbling");
                    }
                    else if (e.InputStreamEnded)
                    {
                        Console.WriteLine("Input stream ended early");
                    }
                    else if (e.Result != null)
                    {
                        Console.WriteLine("Grammar = {0}; Text = {1}; Confidence = {2}", e.Result.Grammar.Name, e.Result.Text, e.Result.Confidence);
                    }
                    else
                    {
                        Console.WriteLine("No result");
                    }
                   */
                    completed = true;
                };
                // set input to constantly re-written stream.
                recognizer.SetInputToAudioStream(buffStream, new SpeechAudioFormatInfo(16000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));

                // Start asynchronous, continuous speech recognition.  
                recognizer.RecognizeAsync(RecognizeMode.Multiple);


                while (!completed) // this keeps the thread alive
                {
                    if (canceled)
                    {
                        wavein.StopRecording();
                        buffStream.Close();// you must close the stream before trying to cancel the async recognition. Else it freezes.
                        recognizer.RecognizeAsyncCancel();
                        threadSpeakRecognition = new Thread(remoteRec);
                        buffStream  = new SpeechStreamer(4096); // I need to do this to make it so one is able to restart the thread, else you get a _writeEvent = null error. No clue why.
                        break;
                    }
                    if (closed)
                    {
                        wavein.StopRecording();
                        buffStream.Close(); // you must close the stream before trying to cancel the async recognition. Else it freezes.
                        recognizer.RecognizeAsyncCancel();
                        threadSpeakRecognition = new Thread(remoteRec);
                        break;
                    }
                    Thread.Sleep(600);
                }
                //recognizer.RecognizeAsyncCancel(); // kill any recognition stuff (it tends to freeze)
            }

        }
        void SpeechRecognizedHandler(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null && e.Result.Text != null)
            {
                //Console.WriteLine("  Recognized text =  {0}", e.Result.Text);
                string temp = e.Result.Text;
                WriteSafe(temp);
                playQueue.Add(temp); // add strings to readout list, to create a basic queue system
            }
            else
            {
                Console.WriteLine("  Recognized text not available.");
            }
        }

        void RecognizeCompletedHandler(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine("  Error encountered, {0}: {1}",
                  e.Error.GetType().Name, e.Error.Message);
            }
            if (e.Cancelled)
            {
                Console.WriteLine("  Operation cancelled.");
            }
            if (e.InputStreamEnded)
            {
                Console.WriteLine("  End of stream encountered.");
            }

            //completed = true;
        }


    }




    class SpeechStreamer : Stream // custom stream wrap to create a cyclical buffer, thanks Sean from stack overflow! https://stackoverflow.com/questions/1682902/streaming-input-to-system-speech-recognition-speechrecognitionengine
    {
        private AutoResetEvent _writeEvent;
        private List<byte> _buffer;
        private int _buffersize;
        private int _readposition;
        private int _writeposition;
        private bool _reset;

        public SpeechStreamer(int bufferSize)
        {
            _writeEvent = new AutoResetEvent(false);
            _buffersize = bufferSize;
            _buffer = new List<byte>(_buffersize);
            for (int i = 0; i < _buffersize; i++)
                _buffer.Add(new byte());
            _readposition = 0;
            _writeposition = 0;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return -1L; }
        }

        public override long Position
        {
            get { return 0L; }
            set { }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0L;
        }

        public override void SetLength(long value)
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int i = 0;
            while (i < count && _writeEvent != null)
            {
                if (!_reset && _readposition >= _writeposition)
                {
                    _writeEvent.WaitOne(100, true);
                    continue;
                }
                buffer[i] = _buffer[_readposition + offset];
                _readposition++;
                if (_readposition == _buffersize)
                {
                    _readposition = 0;
                    _reset = false;
                }
                i++;
            }

            return count;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++)
            {
                _buffer[_writeposition] = buffer[i];
                _writeposition++;
                if (_writeposition == _buffersize)
                {
                    _writeposition = 0;
                    _reset = true;
                }
            }
            _writeEvent.Set();

        }

        public override void Close()
        {
            _writeEvent.Close();
            _writeEvent = null;
            base.Close();
        }

        public override void Flush()
        {

        }
    }
}
