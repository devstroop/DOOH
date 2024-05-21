namespace DOOH.Server.Models
{
    // sample1: {"streams":[{"index":0,"codec_name":"h264","codec_long_name":"unknown","profile":100,"codec_type":"video","codec_time_base":"1/60","codec_tag_string":"avc1","codec_tag":"0x31637661","width":1080,"height":1920,"coded_width":1088,"coded_height":1920,"has_b_frames":1,"sample_aspect_ratio":"1:1","display_aspect_ratio":"9:16","pix_fmt":"yuv420p","level":40,"color_range":"tv","color_space":"bt709","color_transfer":"bt709","color_primaries":"bt709","chroma_location":"left","field_order":"unknown","timecode":"N/A","refs":1,"is_avc":"true","nal_length_size":4,"id":"N/A","r_frame_rate":"30/1","avg_frame_rate":"30/1","time_base":"1/15360","start_pts":0,"start_time":0,"duration_ts":358400,"duration":23.333333,"bit_rate":4454779,"max_bit_rate":"N/A","bits_per_raw_sample":8,"nb_frames":700,"nb_read_frames":"N/A","nb_read_packets":"N/A","tags":{"language":"und","handler_name":"ISO Media file produced by Google Inc."},"disposition":{"default":1,"dub":0,"original":0,"comment":0,"lyrics":0,"karaoke":0,"forced":0,"hearing_impaired":0,"visual_impaired":0,"clean_effects":0,"attached_pic":0,"timed_thumbnails":0}},{"index":1,"codec_name":"aac","codec_long_name":"unknown","profile":1,"codec_type":"audio","codec_time_base":"1/44100","codec_tag_string":"mp4a","codec_tag":"0x6134706d","sample_fmt":"fltp","sample_rate":44100,"channels":2,"channel_layout":"stereo","bits_per_sample":0,"id":"N/A","r_frame_rate":"0/0","avg_frame_rate":"0/0","time_base":"1/44100","start_pts":0,"start_time":0,"duration_ts":1028096,"duration":23.312834,"bit_rate":128109,"max_bit_rate":128109,"bits_per_raw_sample":"N/A","nb_frames":1004,"nb_read_frames":"N/A","nb_read_packets":"N/A","tags":{"language":"eng","handler_name":"ISO Media file produced by Google Inc."},"disposition":{"default":1,"dub":0,"original":0,"comment":0,"lyrics":0,"karaoke":0,"forced":0,"hearing_impaired":0,"visual_impaired":0,"clean_effects":0,"attached_pic":0,"timed_thumbnails":0}}],"format":{"filename":"/tmp/7bbbc810-file","nb_streams":2,"nb_programs":0,"format_name":"mov,mp4,m4a,3gp,3g2,mj2","format_long_name":"unknown","start_time":0,"duration":23.334,"size":13389686,"bit_rate":4590618,"probe_score":100,"tags":{"major_brand":"isom","minor_version":"512","compatible_brands":"isomiso2avc1mp41","encoder":"Lavf58.45.100"}},"chapters":[]}
    // sample2: {"streams":[{"index":0,"codec_name":"png","codec_long_name":"unknown","profile":"unknown","codec_type":"video","codec_time_base":"0/1","codec_tag_string":"[0][0][0][0]","codec_tag":"0x0000","width":497,"height":672,"coded_width":497,"coded_height":672,"has_b_frames":0,"sample_aspect_ratio":"N/A","display_aspect_ratio":"N/A","pix_fmt":"pal8","level":"-99","color_range":"pc","color_space":"unknown","color_transfer":"unknown","color_primaries":"unknown","chroma_location":"unspecified","field_order":"unknown","timecode":"N/A","refs":1,"id":"N/A","r_frame_rate":"25/1","avg_frame_rate":"0/0","time_base":"1/25","start_pts":"N/A","start_time":"N/A","duration_ts":"N/A","duration":"N/A","bit_rate":"N/A","max_bit_rate":"N/A","bits_per_raw_sample":"N/A","nb_frames":"N/A","nb_read_frames":"N/A","nb_read_packets":"N/A","disposition":{"default":0,"dub":0,"original":0,"comment":0,"lyrics":0,"karaoke":0,"forced":0,"hearing_impaired":0,"visual_impaired":0,"clean_effects":0,"attached_pic":0,"timed_thumbnails":0}}],"format":{"filename":"/tmp/3872ebc4-file","nb_streams":1,"nb_programs":0,"format_name":"png_pipe","format_long_name":"unknown","start_time":"N/A","duration":"N/A","size":109233,"bit_rate":"N/A","probe_score":99},"chapters":[]}
    public class ProbeData
    {
        public List<ProbeDataStream> Streams { get; set; }
        public ProbeDataFormat Format { get; set; }
        public List<object> Chapters { get; set; }
    }

    public class ProbeDataStream
    {
        public string Index { get; set; }
        public string CodecName { get; set; }
        public string CodecLongName { get; set; }
        public string Profile { get; set; }
        public string CodecType { get; set; }
        public string CodecTimeBase { get; set; }
        public string CodecTagString { get; set; }
        public string CodecTag { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string CodedWidth { get; set; }
        public string CodedHeight { get; set; }
        public string HasBFrames { get; set; }
        public string SampleAspectRatio { get; set; }
        public string DisplayAspectRatio { get; set; }
        public string PixFmt { get; set; }
        public string Level { get; set; }
        public string ColorRange { get; set; }
        public string ColorSpace { get; set; }
        public string ColorTransfer { get; set; }
        public string ColorPrimaries { get; set; }
        public string ChromaLocation { get; set; }
        public string FieldOrder { get; set; }
        public string Timecode { get; set; }
        public string Refs { get; set; }
        public string IsAVC { get; set; }
        public string NalLengthSize { get; set; }
        public string Id { get; set; }
        public string RFrameRate { get; set; }
        public string AvgFrameRate { get; set; }
        public string TimeBase { get; set; }
        public string StartPts { get; set; }
        public string StartTime { get; set; }
        public string DurationTs { get; set; }
        public string Duration { get; set; }
        public string BitRate { get; set; }
        public string MaxBitRate { get; set; }
        public string BitsPerRawSample { get; set; }
        public string NbFrames { get; set; }
        public string NbReadFrames { get; set; }
        public string NbReadPackets { get; set; }
        public ProbeDataTags Tags { get; set; }
        public ProbeDataDisposition Disposition { get; set; }
    }

    public class ProbeDataTags
    {
        public string Language { get; set; }
        public string HandlerName { get; set; }
    }

    public class ProbeDataDisposition
    {
        public string Default { get; set; }
        public string Dub { get; set; }
        public string Original { get; set; }
        public string Comment { get; set; }
        public string Lyrics { get; set; }
        public string Karaoke { get; set; }
        public string Forced { get; set; }
        public string HearingImpaired { get; set; }
        public string VisualImpaired { get; set; }
        public string CleanEffects { get; set; }
        public string AttachedPic { get; set; }
        public string TimedThumbnails { get; set; }
    }

    public class ProbeDataFormat
    {
        public string Filename { get; set; }
        public string NbStreams { get; set; }
        public string NbPrograms { get; set; }
        public string FormatName { get; set; }
        public string FormatLongName { get; set; }
        public string StartTime { get; set; }
        public string Duration { get; set; }
        public string Size { get; set; }
        public string BitRate { get; set; }
        public string ProbeScore { get; set; }
        public ProbeDataTags Tags { get; set; }
    }

}
