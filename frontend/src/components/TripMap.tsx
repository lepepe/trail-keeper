'use client';

import { useEffect, useMemo } from 'react';
import { MapContainer, TileLayer, Marker, Polyline, Popup, useMap } from 'react-leaflet';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import { Point, Path } from '@/lib/api';

// Fix Leaflet default icon issue
delete (L.Icon.Default.prototype as unknown as { _getIconUrl?: () => string })._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-icon-2x.png',
  iconUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-icon.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-shadow.png',
});

interface TripMapProps {
  points: Point[];
  paths: Path[];
}

function getMarkerIcon(color?: string): L.DivIcon {
  const iconColor = color || 'blue';
  const colorMap: Record<string, string> = {
    green: '#22c55e',
    red: '#ef4444',
    blue: '#3b82f6',
    orange: '#f97316',
    purple: '#a855f7',
  };
  const hexColor = colorMap[iconColor] || colorMap.blue;

  return L.divIcon({
    className: 'custom-marker',
    html: `<div style="
      background-color: ${hexColor};
      width: 24px;
      height: 24px;
      border-radius: 50%;
      border: 3px solid white;
      box-shadow: 0 2px 5px rgba(0,0,0,0.3);
    "></div>`,
    iconSize: [24, 24],
    iconAnchor: [12, 12],
  });
}

function MapBounds({ points, paths }: { points: Point[]; paths: Path[] }) {
  const map = useMap();

  useEffect(() => {
    const allCoords: [number, number][] = [
      ...points.map((p) => [p.latitude, p.longitude] as [number, number]),
      ...paths.map((p) => [p.latitude, p.longitude] as [number, number]),
    ];

    if (allCoords.length > 0) {
      const bounds = L.latLngBounds(allCoords);
      map.fitBounds(bounds, { padding: [50, 50] });
    }
  }, [map, points, paths]);

  return null;
}

export default function TripMap({ points, paths }: TripMapProps) {
  const center = useMemo<[number, number]>(() => {
    if (paths.length > 0) {
      const avgLat = paths.reduce((sum, p) => sum + p.latitude, 0) / paths.length;
      const avgLon = paths.reduce((sum, p) => sum + p.longitude, 0) / paths.length;
      return [avgLat, avgLon];
    }
    if (points.length > 0) {
      const avgLat = points.reduce((sum, p) => sum + p.latitude, 0) / points.length;
      const avgLon = points.reduce((sum, p) => sum + p.longitude, 0) / points.length;
      return [avgLat, avgLon];
    }
    return [30.28, -82.92]; // Default center
  }, [points, paths]);

  const pathCoords = useMemo<[number, number][]>(
    () => paths.map((p) => [p.latitude, p.longitude]),
    [paths]
  );

  return (
    <MapContainer
      center={center}
      zoom={10}
      style={{ height: '100%', width: '100%' }}
    >
      <TileLayer
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />
      <MapBounds points={points} paths={paths} />

      {/* Route polyline */}
      {pathCoords.length > 0 && (
        <Polyline
          positions={pathCoords}
          pathOptions={{ color: '#00FFFF', weight: 6, opacity: 0.9 }}
        />
      )}

      {/* Markers */}
      {points.map((point) => (
        <Marker
          key={point.id}
          position={[point.latitude, point.longitude]}
          icon={getMarkerIcon(point.color)}
        >
          <Popup>{point.name}</Popup>
        </Marker>
      ))}
    </MapContainer>
  );
}
