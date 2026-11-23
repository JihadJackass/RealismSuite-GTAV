using System;
using System.IO;

namespace SPAII
{
	internal class WaveStream : Stream
	{
		private BinaryReader reader;

		private byte[] header;

		private int headerOffset;

		private int _volume;

		private const int MaxVolume = 100;

		public override bool CanSeek => false;

		public override bool CanRead => !IsClosed;

		public override bool CanWrite => false;

		private bool IsClosed => reader == null;

		public override long Position
		{
			get
			{
				CheckDisposed();
				throw new NotSupportedException();
			}
			set
			{
				CheckDisposed();
				throw new NotSupportedException();
			}
		}

		public override long Length
		{
			get
			{
				CheckDisposed();
				throw new NotSupportedException();
			}
		}

		public int Volume
		{
			get
			{
				CheckDisposed();
				return _volume;
			}
			set
			{
				CheckDisposed();
				if (value < 0 || 100 < value)
				{
					throw new ArgumentOutOfRangeException("Volume", value, $"0から{100}の範囲の値を指定してください");
				}
				_volume = value;
			}
		}

		public WaveStream(Stream baseStream)
		{
			headerOffset = 0;
			_volume = 100;
			if (baseStream == null)
			{
				throw new ArgumentNullException("baseStream");
			}
			if (!baseStream.CanRead)
			{
				throw new ArgumentException("読み込み可能なストリームを指定してください", "baseStream");
			}
			reader = new BinaryReader(baseStream);
			ReadHeader();
		}

		public override void Close()
		{
			if (reader != null)
			{
				reader.Close();
				reader = null;
			}
		}

		private void ReadHeader()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				byte[] buffer = reader.ReadBytes(12);
				binaryWriter.Write(buffer);
				while (true)
				{
					byte[] array = reader.ReadBytes(8);
					binaryWriter.Write(array);
					int num = BitConverter.ToInt32(array, 0);
					int count = BitConverter.ToInt32(array, 4);
					if (num == 1635017060)
					{
						break;
					}
					binaryWriter.Write(reader.ReadBytes(count));
				}
				binaryWriter.Close();
				header = memoryStream.ToArray();
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			CheckDisposed();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", offset, "0以上の値を指定してください");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "0以上の値を指定してください");
			}
			checked
			{
				if (buffer.Length - count < offset)
				{
					throw new ArgumentException("配列の範囲を超えてアクセスしようとしました", "offset");
				}
				if (header == null)
				{
					int num = unchecked(count / 2);
					int count2 = num * 2;
					int num2 = reader.Read(buffer, offset, count2);
					if (num2 == 0)
					{
						return 0;
					}
					int num3 = num - 1;
					for (int i = 0; i <= num3; i++)
					{
						short num4;
						unchecked
						{
							num4 = (short)(buffer[offset] | (short)(buffer[checked(offset + 1)] << 8));
						}
						num4 = (short)unchecked(checked(num4 * _volume) / 100);
						buffer[offset] = (byte)(num4 & 0xFF);
						buffer[offset + 1] = (byte)(unchecked((short)(num4 >> 8)) & 0xFF);
						offset += 2;
					}
					return num2;
				}
				int num5 = Math.Min(header.Length - headerOffset, count);
				Buffer.BlockCopy(header, headerOffset, buffer, offset, num5);
				headerOffset += num5;
				if (headerOffset == header.Length)
				{
					header = null;
				}
				return num5;
			}
		}

		public override void SetLength(long value)
		{
			CheckDisposed();
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			CheckDisposed();
			throw new NotSupportedException();
		}

		public override void Flush()
		{
			CheckDisposed();
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			CheckDisposed();
			throw new NotSupportedException();
		}

		private void CheckDisposed()
		{
			if (IsClosed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
		}
	}
}
