using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SoloDevApp.Service.Utilities;

namespace SoloDevApp.Service.Services
{
    public interface ITrackingNguoiDungService : IService<TrackingNguoiDung, TrackingNguoiDungViewModel>
    {
        Task<ResponseEntity> getThongBaoNguoiDung();
    }
    public class TrackingNguoiDungService : ServiceBase<TrackingNguoiDung, TrackingNguoiDungViewModel>, ITrackingNguoiDungService
    {
        private readonly ITrackingNguoiDungRepository _trackingNguoiDungRepository;
        public TrackingNguoiDungService(ITrackingNguoiDungRepository trackingNguoiDungRepository,
            IMapper mapper)
            : base(trackingNguoiDungRepository, mapper)
        {
            _trackingNguoiDungRepository = trackingNguoiDungRepository;
        }

        public async Task<ResponseEntity> getThongBaoNguoiDung()
        {
            try
            {
                //Tam set cung maNguoiDung de test
                string maNguoiDung = "d9699b77-f003-42c4-b050-26607079a789";

                int soBaiTapChuaNop = 0;
                int soVideoChuaXem = 0;

                IEnumerable<TrackingNguoiDung> lsTrackingNguoiDung = await _trackingNguoiDungRepository.GetMultiByConditionAsync("MaNguoiDung", maNguoiDung);



                if (lsTrackingNguoiDung == null)
                {
                    return null;
                }

                List<ThongBaoTrackingViewModel> lsThongBao = new List<ThongBaoTrackingViewModel>();

                foreach (var item in lsTrackingNguoiDung.GroupBy(x=>x.LoaiHanhDong)){
                    switch (item.Key)
                    {
                        case "BAI_TAP":
                            {
                                //Object gồm có 2 key là lsThongBao và soBaiTapChuaNop
                                var result = KiemTraTrackingBaiTap(item);
                                lsThongBao.AddRange(result.lsThongBao);
                                soBaiTapChuaNop = result.soBaiTapChuaNop;
                            }
                            break;
                        case "VIDEO_XEM_LAI":
                            {
                                //Object gồm có 2 key là lsThongBao và soVideoChuaXem
                                var result = KiemTraTrackingVideoXemLai(item);
                                lsThongBao.AddRange(result.lsThongBao);
                                soVideoChuaXem = result.soVideoChuaXem;
                            }
                            break;
                    }
                }

                if (soBaiTapChuaNop == 1)
                {
                    lsThongBao.Add(new ThongBaoTrackingViewModel("Bạn đã không nộp bài tập, hãy cố gắng làm bài tập nhé", "DONG_LUC", DateTime.Now));
                } else if (soBaiTapChuaNop >= 2)
                {
                    lsThongBao.Add(new ThongBaoTrackingViewModel("Bạn đã liên tiếp không làm bài tập nhiều lần, nếu cứ tiếp tục bạn sẽ không thể học nổi nội dung tiếp theo, hãy cố gắng nhé", "DONG_LUC", DateTime.Now));

                } 
                
                if (soBaiTapChuaNop == 0 && soVideoChuaXem == 0)
                {
                    lsThongBao.Add(new ThongBaoTrackingViewModel("Hệ thống nhận thấy bjan đang học rất tốt, bạn hãy cố gắng nhé", "DONG_LUC", DateTime.Now));
                } else if (soVideoChuaXem > 0 && soBaiTapChuaNop > 0)
                {
                    lsThongBao.Add(new ThongBaoTrackingViewModel("Hệ thống nhận thấy bạn đang học không tốt (không nộp bài, không xem lại video), bạn hãy dàn nhiều thời gian để học hơn nhé", "THONG_BAO_GHIM", DateTime.Now));
                }

                return new ResponseEntity(StatusCodeConstants.OK, lsThongBao);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }


        }

        //Trả thêm số bài tập chưa nộp nên trả ra dạng dynamic 1 object
        private dynamic KiemTraTrackingBaiTap (IEnumerable<TrackingNguoiDung> lsTracking)
        {
            List<ThongBaoTrackingViewModel> lsThongBao = new List<ThongBaoTrackingViewModel> ();

            int soBaiTapChuaNop = 0;

            foreach (var item in lsTracking)
            {
                //Lấy ra chi tiết hành động của từng record thành list object
                List<ChiTietHanhDongBaiTapViewModel> lsChiTietHanhDongVm = JsonConvert.DeserializeObject<List<ChiTietHanhDongBaiTapViewModel>>(item.ChiTietHanhDong);

                //Duyệt từng chi tiết hành động tạo ra thông báo
                foreach (var chiTietHanhDong in lsChiTietHanhDongVm)
                {

                    int ngayHetHan = FuncUtilities.TinhKhoangCachNgay(chiTietHanhDong.NgayHetHan);

                    if (chiTietHanhDong.DaNop == false)
                    {
                        soBaiTapChuaNop++;
                    }


                    if (ngayHetHan > 0 && chiTietHanhDong.DaNop == false)
                    {
                        lsThongBao.Add(new ThongBaoTrackingViewModel($"Bài tập {chiTietHanhDong.TieuDe} còn {ngayHetHan} ngày nữa sẽ hết hạn", "BAI_TAP", chiTietHanhDong.NgayThang));
                    }

                    if (ngayHetHan < 0 && chiTietHanhDong.DaNop == false)
                    {
                        lsThongBao.Add(new ThongBaoTrackingViewModel($"Bài tập {chiTietHanhDong.TieuDe} đã hết hạn nộp bài tập", "BAI_TAP", chiTietHanhDong.NgayThang));
                    }

                    if (chiTietHanhDong.DaNop && chiTietHanhDong.DaCham == false)
                    {
                        lsThongBao.Add(new ThongBaoTrackingViewModel($"Bạn đã nộp bài tập {chiTietHanhDong.TieuDe} thành công bạn vui lòng đợi mentor chấm bài", "BAI_TAP", chiTietHanhDong.NgayThang));
                    }

                    if (chiTietHanhDong.DaCham)
                    {
                        lsThongBao.Add(new ThongBaoTrackingViewModel($"Bài tập {chiTietHanhDong.TieuDe} đã được chấm. Bạn được {chiTietHanhDong.Diem} điểm", "BAI_TAP", chiTietHanhDong.NgayThang));
                    }
                }
            }

            return new { lsThongBao = lsThongBao, soBaiTapChuaNop = soBaiTapChuaNop };

        }


        //Trả thêm số video chưa xem nên trả ra dạng dynamic 1 object
        private dynamic KiemTraTrackingVideoXemLai(IEnumerable<TrackingNguoiDung> lsTracking)
        {
            List<ThongBaoTrackingViewModel> lsThongBao = new List<ThongBaoTrackingViewModel>();

            int soVideoChuaXem = 0;

            foreach (var item in lsTracking)
            {
                //Lấy ra chi tiết hành động của từng record thành list object
                List<ChiTietHanhDongVideoXemLaiViewModel> lsChiTietHanhDongVm = JsonConvert.DeserializeObject<List<ChiTietHanhDongVideoXemLaiViewModel>>(item.ChiTietHanhDong);

                //Duyệt từng chi tiết hành động tạo ra thông báo
                foreach (var chiTietHanhDong in lsChiTietHanhDongVm)
                {

                    int ngayHetHan = FuncUtilities.TinhKhoangCachNgay(chiTietHanhDong.NgayHetHan);

                    if (chiTietHanhDong.DaXem == false)
                    {
                        soVideoChuaXem++;
                    }


                    if (ngayHetHan <= 0 && chiTietHanhDong.DaXem == false) 
                    {
                        lsThongBao.Add(new ThongBaoTrackingViewModel($"Video {chiTietHanhDong.TieuDe} đã hết hạn", "VIDEO_XEM_LAI", chiTietHanhDong.NgayThang));
                    }

                    if (ngayHetHan > 0 && chiTietHanhDong.DaXem == false)
                    {
                        lsThongBao.Add(new ThongBaoTrackingViewModel($"Video {chiTietHanhDong.TieuDe} còn {ngayHetHan} ngày nữa sẽ hết hạn và bị xóa", "VIDEO_XEM_LAI", chiTietHanhDong.NgayThang));
                    }

                }
            }

            return new { lsThongBao = lsThongBao, soVideoChuaXem = soVideoChuaXem };

        }
    }
}
