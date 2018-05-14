using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableConcurrentQueue;

namespace Tests
{
    [TestClass]
    public class ObservableConcurrentQueueTest
    {
        #region Fields

        /// <summary>
        ///     Gets or sets a value indicating whether the queue is empty.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the queue instance is empty; otherwise, <c>false</c>.
        /// </value>
        private bool _isQueueEmpty = true;

        /// <summary>
        ///     The queue
        /// </summary>
        private ObservableConcurrentQueue<int> _queue;

        /// <summary>
        ///     The queue new item.
        /// </summary>
        private int _queueAddedItem;

        /// <summary>
        ///     The queue deleted item.
        /// </summary>
        private int _queueDeletedItem;

        /// <summary>
        ///     The queue peeked item
        /// </summary>
        private int _queuePeekedItem;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the test.
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            this._queue = new ObservableConcurrentQueue<int>();
            this._queue.ContentChanged += this.OnQueueChanged;
        }

        /// <summary>
        ///     Mains the test.
        /// </summary>
        [TestMethod]
        public void MainTest()
        {
            // Add 2 elements.
            this.EnqueueEventTest();

            // Dequeue 1 element.
            this.DequeueEventTest();

            // Peek 1 element.
            this.PeekEventTest();

            // Dequeue all elements
            // the queue should be empty
            this.EmptyQueueTest();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     De-queue event test.
        /// </summary>
        private void DequeueEventTest()
        {
            var result = this._queue.TryDequeue(out var item);
            Assert.IsTrue(result);
            Assert.AreEqual(item, this._queueDeletedItem);
            Assert.IsFalse(this._isQueueEmpty);
        }

        /// <summary>
        ///     Empties the queue test.
        /// </summary>
        private void EmptyQueueTest()
        {
            while (this._queue.TryDequeue(out var item))
            {
                Assert.AreEqual(item, this._queueDeletedItem);
            }

            Assert.IsTrue(this._isQueueEmpty);
        }

        /// <summary>
        ///     Enqueues the event test.
        /// </summary>
        private void EnqueueEventTest()
        {
            const int item = 11;
            this._queue.Enqueue(item);
            Assert.AreEqual(item, this._queueAddedItem);
            Assert.IsFalse(this._isQueueEmpty);

            this._queue.Enqueue(item + 1);
            Assert.AreEqual(item + 1, this._queueAddedItem);
            Assert.IsFalse(this._isQueueEmpty);
        }

        /// <summary>
        /// The on queue changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private void OnQueueChanged(object sender, NotifyConcurrentQueueChangedEventArgs<int> args)
        {
            switch (args.Action)
            {
                case NotifyConcurrentQueueChangedAction.Enqueue:
                    {
                        this._queueAddedItem = args.ChangedItem;
                        this._isQueueEmpty = false;
                        break;
                    }

                case NotifyConcurrentQueueChangedAction.Dequeue:
                    {
                        this._queueDeletedItem = args.ChangedItem;
                        this._isQueueEmpty = false;
                        break;
                    }

                case NotifyConcurrentQueueChangedAction.Peek:
                    {
                        this._queuePeekedItem = args.ChangedItem;
                        this._isQueueEmpty = false;
                        break;
                    }

                case NotifyConcurrentQueueChangedAction.Empty:
                    {
                        this._isQueueEmpty = true;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Peeks the event test.
        /// </summary>
        private void PeekEventTest()
        {
            var result = this._queue.TryPeek(out var item);
            Assert.IsTrue(result);
            Assert.AreEqual(item, this._queuePeekedItem);
            Assert.IsFalse(this._isQueueEmpty);
        }

        #endregion
    }
}
