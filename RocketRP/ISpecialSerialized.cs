namespace RocketRP
{
	public interface ISpecialSerialized
	{
		void Deserialize(BinaryReader br, IFileVersionInfo versionInfo);

		void Serialize(BinaryWriter bw, IFileVersionInfo versionInfo);
	}
}
