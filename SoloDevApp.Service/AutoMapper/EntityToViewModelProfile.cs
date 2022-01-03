﻿using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;

namespace SoloDevApp.Service.AutoMapper
{
    public class EntityToViewModelProfile : Profile
    {
        public EntityToViewModelProfile()
        {
            /*=========== HỆ THỐNG =============*/
            CreateMap<Quyen, QuyenViewModel>();
            CreateMap<NhomQuyen, NhomQuyenViewModel>()
                .ForMember(modelVm => modelVm.DanhSachQuyen,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<string>>(entity.DanhSachQuyen)));

            /*=========== LỘ TRÌNH =============*/
            CreateMap<LoTrinh, ThongTinLoTrinhViewModel>()
                .ForMember(modelVm => modelVm.DanhSachKhoaHoc,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<dynamic>>(entity.DanhSachKhoaHoc)))
                .ForMember(modelVm => modelVm.DanhSachLopHoc,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<dynamic>>(entity.DanhSachLopHoc)));
            CreateMap<LoTrinh, LoTrinhViewModel>()
                .ForMember(modelVm => modelVm.DanhSachKhoaHoc,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<dynamic>>(entity.DanhSachKhoaHoc)))
                .ForMember(modelVm => modelVm.DanhSachLopHoc,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<dynamic>>(entity.DanhSachLopHoc)));

            /*=========== LỚP HỌC =============*/
            CreateMap<LopHoc, LopHocViewModel>()
                .ForMember(modelVm => modelVm.NgayBatDau,
                                m => m.MapFrom(entity => FuncUtilities.ConvertDateToString(entity.NgayBatDau)))
                .ForMember(modelVm => modelVm.NgayKetThuc,
                                m => m.MapFrom(entity => FuncUtilities.ConvertDateToString(entity.NgayKetThuc)))
                .ForMember(modelVm => modelVm.DanhSachGiangVien,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<HashSet<dynamic>>(entity.DanhSachGiangVien)))
            .ForMember(modelVm => modelVm.DanhSachHocVien,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<HashSet<dynamic>>(entity.DanhSachHocVien)))
            .ForMember(modelVm => modelVm.DanhSachMentor,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<HashSet<dynamic>>(entity.DanhSachMentor)));
            CreateMap<LopHoc, ThongTinLopHocViewModel>()
                .ForMember(modelVm => modelVm.NgayBatDau,
                                m => m.MapFrom(entity => FuncUtilities.ConvertDateToString(entity.NgayBatDau)))
                .ForMember(modelVm => modelVm.NgayKetThuc,
                                m => m.MapFrom(entity => FuncUtilities.ConvertDateToString(entity.NgayKetThuc)))
                .ForMember(modelVm => modelVm.DanhSachGiangVien,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<HashSet<dynamic>>(entity.DanhSachGiangVien)))
            .ForMember(modelVm => modelVm.DanhSachHocVien,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<HashSet<dynamic>>(entity.DanhSachHocVien)))
            .ForMember(modelVm => modelVm.DanhSachMentor,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<HashSet<dynamic>>(entity.DanhSachMentor)));

            /*=========== KHÓA HỌC =============*/
            CreateMap<KhoaHoc, KhoaHocViewModel>()
                .ForMember(modelVm => modelVm.DanhSachLoTrinh,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<HashSet<int>>(entity.DanhSachLoTrinh)))
                .ForMember(modelVm => modelVm.DanhSachChuongHoc,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<int>>(entity.DanhSachChuongHoc)));
            CreateMap<KhoaHoc, ThongTinKhoaHocViewModel>()
                .ForMember(modelVm => modelVm.DanhSachLoTrinh,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<HashSet<dynamic>>(entity.DanhSachLoTrinh)))
                .ForMember(modelVm => modelVm.DanhSachChuongHoc,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<dynamic>>(entity.DanhSachChuongHoc)));

            /*=========== CHƯƠNG HỌC =============*/
            CreateMap<ChuongHoc, ChuongHocViewModel>()
                .ForMember(modelVm => modelVm.DanhSachBaiHoc,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<dynamic>>(entity.DanhSachBaiHoc)));
            CreateMap<ChuongHoc, ThongTinChuongHocViewModel>()
                .ForMember(modelVm => modelVm.DanhSachBaiHoc,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<dynamic>>(entity.DanhSachBaiHoc)));

            /*=========== BÀI HỌC =============*/
            CreateMap<BaiHoc, BaiHocViewModel>()
                .ForMember(modelVm => modelVm.DanhSachCauHoi,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<dynamic>>(entity.DanhSachCauHoi)));
            CreateMap<LoaiBaiHoc, LoaiBaiHocViewModel>();

            /*=========== BÀI TẬP =============*/
            CreateMap<BaiTap, BaiTapViewModel>();
            CreateMap<BaiTapNop, BaiTapNopViewModel>();

            /*=========== CÂU HỎI =============*/
            CreateMap<CauHoi, CauHoiViewModel>()
                .ForMember(modelVm => modelVm.CauTraLoi,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<CauTraLoiViewModel>>(entity.CauTraLoi)))
                .ForMember(modelVm => modelVm.CauHoiLienQuan,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<List<CauHoiLienQuanViewModel>>(entity.CauHoiLienQuan)));

            /*=========== CHUYỂN LỚP =============*/
            CreateMap<ChuyenLop, ChuyenLopViewModel>();

            /*=========== TÀI KHOẢN =============*/
            CreateMap<NguoiDung, NguoiDungViewModel>();

            /*=========== KHÁCH HÀNG =============*/
            CreateMap<KhachHang, KhachHangViewModel>()
                .ForMember(modelVm => modelVm.ThongTinKH,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<ThongTinKHViewModel>(entity.ThongTinKH)))
                .ForMember(modelVm => modelVm.DiaChi,
                                m => m.MapFrom(entity => JsonConvert.DeserializeObject<DiaChiViewModel>(entity.DiaChi)));
            CreateMap<HocPhi, HocPhiViewModel>();
            CreateMap<LopHoc_TaiLieu, LopHoc_TaiLieuViewModel>();
            CreateMap<GhiChuUser, GhiChuUserViewModel>();
            CreateMap<ChungChi, ChungChiViewModel>();
            CreateMap<DiemDanh, DiemDanhViewModel>();
            CreateMap<BieuMau, BieuMauViewModel>();
            CreateMap<ChiNhanh, ChiNhanhViewModel>();
            CreateMap<XepLich, XepLichViewModel>();
            CreateMap<XemLaiBuoiHoc, XemLaiBuoiHocViewModel>();

            /*=========== ROAD MAP =============*/
            CreateMap<RoadMap, RoadMapViewModel>();
            CreateMap<RoadMapDetail, RoadMapDetailViewModel>();
            CreateMap<BuoiHoc, BuoiHocViewModel>();
            CreateMap<LoaiBaiTap, LoaiBaiTapViewModel>();
            CreateMap<Skill, SkillViewModel>();
            CreateMap<Unit, UnitViewModel>();
            CreateMap<UnitCourse, UnitCourseViewModel>();
            CreateMap<VideoExtra, VideoExtraViewModel>();
            CreateMap<BaiHoc_TaiLieu_Link_TracNghiem, BaiHoc_TaiLieu_Link_TracNghiemViewModel>();

            /*=========== ROAD MAP VIEW TABLE =============*/
            CreateMap<TaiLieuBaiHoc, TaiLieuBaiHocViewModel>();
            CreateMap<TaiLieuBaiTap,TaiLieuBaiTapViewModel>();
            CreateMap<TaiLieuDocThem, TaiLieuDocThemViewModel>();
            CreateMap<TaiLieuProjectLamThem, TaiLieuProjectLamThemViewModel>();
            CreateMap<TracNghiem, TracNghiemViewModel>();
        }
    }
}