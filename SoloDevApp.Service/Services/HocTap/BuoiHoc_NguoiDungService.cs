using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IBuoiHoc_NguoiDungService : IService<BuoiHoc_NguoiDung, BuoiHoc_NguoiDungViewModel>
    {
        Task<ResponseEntity> NopBaiTap(dynamic modelVm);
    }

    public class BuoiHoc_NguoiDungService : ServiceBase<BuoiHoc_NguoiDung, BuoiHoc_NguoiDungViewModel>, IBuoiHoc_NguoiDungService
    {
        IBuoiHoc_NguoiDungRepository _buoiHoc_NguoiDungRepository;
        ITrackingNguoiDungRepository _trackingNguoiDungRepository;

        public BuoiHoc_NguoiDungService(IBuoiHoc_NguoiDungRepository buoiHoc_NguoiDungRepository, ITrackingNguoiDungRepository trackingNguoiDungRepository,
            IMapper mapper)
            : base(buoiHoc_NguoiDungRepository, mapper)
        {
            _buoiHoc_NguoiDungRepository = buoiHoc_NguoiDungRepository;
            _trackingNguoiDungRepository = trackingNguoiDungRepository;
        }

        public async Task<ResponseEntity> NopBaiTap(dynamic modelVm)
        {
            try
            {
                //Lấy ra MaBuoiHoc và MaNguoiDung từ modelVm để lấy dòng record ra
                List<KeyValuePair<string, dynamic>> colums = new List<KeyValuePair<string, dynamic>>();

                colums.Add(new KeyValuePair<string, dynamic>("MaBuoiHoc", modelVm.MaBuoiHoc));
                colums.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", modelVm.MaNguoiDung));

                BuoiHoc_NguoiDung entity = await _buoiHoc_NguoiDungRepository.GetSingleByListConditionAsync(colums);

                if (entity == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Không tìm thấy lịch sử học tập");
                }

                //Lấy ra danh sách lịch sử học tập
                IEnumerable<LichSuHocTap> lsLichSuHocTap = JsonConvert.DeserializeObject<IEnumerable<LichSuHocTap>>(entity.LichSuHocTap);

                //Lấy ra lich sử của bài tập client gửi lên
                LichSuHocTap lichSuHocTap = lsLichSuHocTap.Where(x => x.MaBaiHoc == modelVm.MaBaiHoc).FirstOrDefault();

                if (entity == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Không tìm thấy lịch sử học tập");
                }

                switch (lichSuHocTap.LoaiBaiTap)
                {
                    
                    case "QUIZ_WRITE":
                        {
                            //Tạo string để lát đẩy vào phần nội dung
                            //Nếu thay đổi thì cần thay đổi view model

                            lichSuHocTap.NoiDung = modelVm.NoiDung;

                            //Set điểm thành -1 để biết là chưa chấm
                            lichSuHocTap.Diem = -1;
                            lichSuHocTap.NgayThang = FuncUtilities.ConvertDateToString(DateTime.Now);
                        }
                        break;
                    case "QUIZ":
                    case "QUIZ-PURPLE":
                        {
                            //Cập nhật điểm vì nộp là bài chấm điểm
                            lichSuHocTap.Diem = modelVm.Diem;
                            lichSuHocTap.NgayThang = FuncUtilities.ConvertDateToString(DateTime.Now);
                        }
                        break;
                    case "CAPSTONE":
                        {
                            //Tạo string để lát đẩy vào phần nội dung
                            //Nếu thay đổi thì cần thay đổi view model
                            string noiDungNop = $"LinkBai : {modelVm.NoiDung.LinkBai},LinkYoutube : {modelVm.NoiDung.LinkYoutube},LinkDeploy : {modelVm.NoiDung.LinkDeploy}";

                            lichSuHocTap.NoiDung = noiDungNop;

                            //Set điểm thành -1 để biết là chưa chấm
                            lichSuHocTap.Diem = -1;
                            lichSuHocTap.NgayThang = FuncUtilities.ConvertDateToString(DateTime.Now);
                        }
                        break;
                }

                //Map ngược lại lịch sử học tập thành string sau đó gán vào lại thằng entity rồi cập nhật vào db
                entity.LichSuHocTap = JsonConvert.SerializeObject(lsLichSuHocTap);
                

                //Xử lý cập nhật thông tin vào tracking người dùng
                List<KeyValuePair<string ,dynamic>> columsTracking = new List<KeyValuePair<string, dynamic>>();

                columsTracking.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", modelVm.MaNguoiDung));
                columsTracking.Add(new KeyValuePair<string, dynamic>("MaLop", modelVm.MaLop));
                columsTracking.Add(new KeyValuePair<string, dynamic>("LoaiHanhDong", "BAI_TAP"));
                

                TrackingNguoiDung trackingBaiTap = await _trackingNguoiDungRepository.GetSingleByListConditionAsync(columsTracking);
                //Chuyển string thành object để xử lý
                List<ChiTietHanhDongBaiTapViewModel> lsChiTietBaiTap = JsonConvert.DeserializeObject < List<ChiTietHanhDongBaiTapViewModel>>(trackingBaiTap.ChiTietHanhDong);

                //Tìm index của bài tập và cập nhật da nop + ngay thang
                int baiTapIndex = lsChiTietBaiTap.FindIndex(item => item.MaBaiHoc == modelVm.MaBaiHoc);
                if (baiTapIndex == -1) {
                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Không có thông tin bài tập trong tracking");
                }

                lsChiTietBaiTap[baiTapIndex].DaNop = true;
                lsChiTietBaiTap[baiTapIndex].NgayThang = DateTime.Now;

                //Covert object về string và update lại record
                trackingBaiTap.ChiTietHanhDong = JsonConvert.SerializeObject(lsChiTietBaiTap);
                trackingBaiTap.NgayCuoiCungHoatDong = DateTime.Now;

                //Ok hết mới ghi vào DB

                var result = await _buoiHoc_NguoiDungRepository.UpdateAsync(entity.Id, entity);

                await _trackingNguoiDungRepository.UpdateAsync(trackingBaiTap.Id, trackingBaiTap);

                return new ResponseEntity(StatusCodeConstants.OK, result);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

       

    }

}
